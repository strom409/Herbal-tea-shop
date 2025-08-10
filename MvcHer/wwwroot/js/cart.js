// Global Shopping Cart Functionality

// Global function to update cart count
function updateCartCount() {
    $.get('/Cart/GetCartCount')
        .done(function(response) {
            const count = response.count || 0;
            $('#cart-count').text(count);
            
            if (count > 0) {
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
    // Simple alert for now - you can enhance this with toast notifications
    if (type === 'success') {
        alert(' ' + message);
    } else {
        alert(' ' + message);
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