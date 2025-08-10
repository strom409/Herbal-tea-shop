// Product Detail Functionality
class ProductDetail {
    constructor() {
        this.currentProduct = null;
        this.init();
    }

    init() {
        this.bindEvents();
    }

    bindEvents() {
        // More detail button click
        $(document).on('click', '.more-detail-btn', (e) => {
            e.preventDefault();
            this.showProductDetail(e.currentTarget);
        });

        // Quantity controls
        $(document).on('click', '#quantity-decrease', (e) => {
            e.preventDefault();
            this.decreaseQuantity();
        });

        $(document).on('click', '#quantity-increase', (e) => {
            e.preventDefault();
            this.increaseQuantity();
        });

        // Add to cart from detail modal
        $(document).on('click', '#product-detail-add-to-cart', (e) => {
            e.preventDefault();
            this.addToCartFromDetail();
        });

        // Quantity input change
        $(document).on('change', '#product-detail-quantity', (e) => {
            const value = parseInt($(e.target).val());
            if (value < 1) {
                $(e.target).val(1);
            } else if (value > 99) {
                $(e.target).val(99);
            }
        });
    }

    showProductDetail(button) {
        const $button = $(button);
        
        // Get product data from button attributes
        this.currentProduct = {
            id: $button.data('product-id'),
            name: $button.data('product-name'),
            price: parseFloat($button.data('product-price')),
            image: $button.data('product-image'),
            description: $button.data('product-description'),
            rating: parseInt($button.data('product-rating'))
        };

        // Populate modal with product data
        $('#product-detail-image').attr('src', this.currentProduct.image).attr('alt', this.currentProduct.name);
        $('#product-detail-name').text(this.currentProduct.name);
        $('#product-detail-price').text(`$${this.currentProduct.price.toFixed(2)}`);
        $('#product-detail-description').text(this.currentProduct.description);
        
        // Generate rating stars
        this.generateRatingStars(this.currentProduct.rating);
        
        // Reset quantity to 1
        $('#product-detail-quantity').val(1);
        
        // Show modal
        $('#productDetailModal').modal('show');
    }

    generateRatingStars(rating) {
        let starsHTML = '';
        for (let i = 1; i <= 5; i++) {
            if (i <= rating) {
                starsHTML += '<small class="fa fa-star text-primary"></small>';
            } else {
                starsHTML += '<small class="fa fa-star text-muted"></small>';
            }
        }
        $('#product-detail-rating').html(starsHTML);
    }

    decreaseQuantity() {
        const $quantity = $('#product-detail-quantity');
        const currentValue = parseInt($quantity.val());
        if (currentValue > 1) {
            $quantity.val(currentValue - 1);
        }
    }

    increaseQuantity() {
        const $quantity = $('#product-detail-quantity');
        const currentValue = parseInt($quantity.val());
        if (currentValue < 99) {
            $quantity.val(currentValue + 1);
        }
    }

    addToCartFromDetail() {
        if (!this.currentProduct) {
            return;
        }

        const quantity = parseInt($('#product-detail-quantity').val());
        
        // Add to cart using the existing cart functionality
        if (window.teaCart) {
            // Add the product multiple times based on quantity
            for (let i = 0; i < quantity; i++) {
                window.teaCart.addToCart(
                    this.currentProduct.id,
                    this.currentProduct.name,
                    this.currentProduct.price,
                    this.currentProduct.image
                );
            }
            
            // Close the product detail modal
            $('#productDetailModal').modal('hide');
            
            // Show success message
            this.showNotification(`${this.currentProduct.name} (${quantity}x) added to cart!`);
        }
    }

    showNotification(message) {
        // Create notification element
        const notification = $(`
            <div class="alert alert-success alert-dismissible fade show position-fixed" 
                 style="top: 20px; right: 20px; z-index: 9999; min-width: 300px;">
                <i class="fa fa-check-circle me-2"></i>
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `);
        
        $('body').append(notification);
        
        // Auto remove after 3 seconds
        setTimeout(() => {
            notification.alert('close');
        }, 3000);
    }
}

// Initialize product detail when document is ready
$(document).ready(function() {
    window.productDetail = new ProductDetail();
}); 