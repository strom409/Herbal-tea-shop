// Global Shopping Cart Functionality

$(document).ready(function () {
    CartCountIcon();
});

function CartCountIcon() {
    // Target the cart count element
    const cartBadge = document.getElementById('cart-count-desktop');
    const cartBadgeMobile = document.getElementById('cart-icon-float');
    const badge = cartBadgeMobile.querySelector('.cart-count');

    if (!cartBadge || !badge) {
        return; // Stop script
    }

    // Create an observer instance
    const observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            const count = parseInt(cartBadge.textContent);
            if (count > 0) {
                badge.textContent = count;
                badge.style.display = 'inline-block';
                $(cartBadge).show();
            } else {
                badge.style.display = 'none';
                badge.textContent = 0;
                $(cartBadge).hide();
            }
        });
    });

    // Configuration of the observer
    const config = {
        childList: true,
        characterData: true,
        subtree: true
    };

    // Start observing
    observer.observe(cartBadge, config);

    // Initial check
    const initialCount = parseInt(cartBadge.textContent);
    if (initialCount > 0) {
        $(cartBadge).show();
    } else {
        $(cartBadge).hide();
    }
}

// Global function to update cart count
function updateCartCount() {
    $.get('/Cart/GetCartCount')
        .done(function (response) {
            const count = response.count || 0;
            $('#cart-count').text(count);
            
            if (count > 0) {
                CartCountIcon();
                $('#cart-count').show();
            } else {
                $('#cart-count').hide();
            }
        })
        .fail(function() {
            console.error('Failed to get cart count');
        });
}

// Global function for opening cart modal
function openCartModal() {
    window.location.href = '/Cart';
}

// Global function to show notifications
function showNotification(type, message) {
    // Create toast container if it doesn't exist
    let toastContainer = document.querySelector('.toast-container');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.className = 'toast-container';
        document.body.appendChild(toastContainer);
    }

    // Create a unique ID for the toast
    const toastId = 'toast-' + Date.now();
    
    // Create toast element with the new design
    const toast = document.createElement('div');
    toast.id = toastId;
    toast.className = `toast show align-items-center text-white bg-${type === 'success' ? 'success' : 'danger'}`;
    toast.role = 'alert';
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');
    
    // Set up the toast content with the new structure
    const icon = type === 'success' ? 'check-circle' : 'exclamation-circle';
    
    toast.innerHTML = `
        <div class="d-flex w-100">
            <div class="toast-body">
                <i class="fas fa-${icon}"></i>
                <div class="toast-content">${message}</div>
            </div>
            <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;
    
    // Add to container
    toastContainer.prepend(toast);
    
    // Auto remove after 4 seconds
    setTimeout(() => {
        const toastElement = document.getElementById(toastId);
        if (toastElement) {
            toastElement.classList.add('hide');
            setTimeout(() => {
                if (toastElement && toastElement.parentNode) {
                    toastElement.remove();
                }
            }, 300);
        }
    }, 4000);
    
    // Close button functionality
    const closeButton = toast.querySelector('.btn-close');
    if (closeButton) {
        closeButton.addEventListener('click', () => {
            toast.classList.add('hide');
            setTimeout(() => {
                if (toast && toast.parentNode) {
                    toast.remove();
                }
            }, 300);
        });
    }
}

// Global function to add item to cart
function addToCart(productId, productName = 'Product', quantity = 1) {
    console.log('Adding to cart:', productId, productName, quantity);
    
    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        data: {
            productId: productId,
            quantity: quantity
        },
        success: function(response) {
            console.log('Add to cart response:', response);
            if (response.success) {
                // Update cart count in navigation
                updateCartCount();
                
                // Show success message
                showNotification('success', productName + ' added to cart successfully!');
            } else {
                showNotification('error', 'Error: ' + response.message);
            }
        },
        error: function(xhr, status, error) {
            console.error('Add to cart error:', error);
            showNotification('error', 'Error adding product to cart');
        }
    });
}

// Server-side Shopping Cart Class
class ShoppingCart {
    constructor() {
        this.init();
    }

    init() {
        this.updateCartIcon();
        this.bindEvents();
    }

    bindEvents() {
        // Cart icon click
        $(document).on('click', '.cart-icon', (e) => {
            e.preventDefault();
            this.openCartModal();
        });
        
        // Bind add to cart buttons
        $(document).on('click', '[data-add-to-cart]', (e) => {
            e.preventDefault();
            const productId = $(e.target).data('product-id');
            const productName = $(e.target).data('product-name') || 'Product';
            const quantity = $(e.target).data('quantity') || 1;
            addToCart(productId, productName, quantity);
        });
    }

    // Get cart count from server
    updateCartIcon() {
        updateCartCount();
    }

    // Open cart modal (redirect to cart page for now)
    openCartModal() {
        window.location.href = '/Cart';
    }
}

// Initialize cart when document is ready
$(document).ready(function() {
    console.log('Cart.js loaded - Initializing shopping cart');
    const cart = new ShoppingCart();
    
    // Update cart count on page load
    updateCartCount();
});