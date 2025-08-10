// Checkout Functionality
class Checkout {
    constructor() {
        this.cart = JSON.parse(localStorage.getItem('teaCart')) || [];
        this.shippingCost = 5.99;
        this.taxRate = 0.08; // 8% tax rate
        this.init();
    }

    init() {
        this.displayOrderSummary();
        this.bindEvents();
        this.validateCart();
    }

    bindEvents() {
        // Place order button
        $(document).on('click', '#place-order-btn', (e) => {
            e.preventDefault();
            this.processOrder();
        });

        // Form validation
        $(document).on('input', '#cardNumber', function() {
            $(this).val($(this).val().replace(/\D/g, '').replace(/(\d{4})/g, '$1 ').trim());
        });

        $(document).on('input', '#expiryDate', function() {
            let value = $(this).val().replace(/\D/g, '');
            if (value.length >= 2) {
                value = value.substring(0, 2) + '/' + value.substring(2, 4);
            }
            $(this).val(value);
        });

        $(document).on('input', '#cvv', function() {
            $(this).val($(this).val().replace(/\D/g, '').substring(0, 4));
        });

        $(document).on('input', '#phone', function() {
            $(this).val($(this).val().replace(/\D/g, ''));
        });
    }

    validateCart() {
        if (this.cart.length === 0) {
            this.showError('Your cart is empty. Please add items before checkout.');
            $('#place-order-btn').prop('disabled', true);
            return false;
        }
        return true;
    }

    displayOrderSummary() {
        const orderSummary = $('#order-summary');
        
        if (this.cart.length === 0) {
            orderSummary.html('<p class="text-center text-muted">No items in cart</p>');
            return;
        }

        let summaryHTML = '';
        let subtotal = 0;

        this.cart.forEach(item => {
            const itemTotal = item.price * item.quantity;
            subtotal += itemTotal;

            summaryHTML += `
                <div class="d-flex justify-content-between align-items-center mb-2">
                    <div class="d-flex align-items-center">
                        <img src="${item.image}" alt="${item.name}" 
                             style="width: 40px; height: 40px; object-fit: cover;" class="me-2">
                        <div>
                            <small class="fw-bold">${item.name}</small>
                            <br>
                            <small class="text-muted">Qty: ${item.quantity}</small>
                        </div>
                    </div>
                    <span class="fw-bold">$${itemTotal.toFixed(2)}</span>
                </div>
            `;
        });

        orderSummary.html(summaryHTML);
        
        const tax = subtotal * this.taxRate;
        const total = subtotal + this.shippingCost + tax;

        $('#subtotal').text(`$${subtotal.toFixed(2)}`);
        $('#tax').text(`$${tax.toFixed(2)}`);
        $('#total').text(`$${total.toFixed(2)}`);
    }

    validateForm() {
        const requiredFields = [
            'firstName', 'lastName', 'email', 'phone', 'address', 
            'city', 'state', 'zipCode', 'cardNumber', 'expiryDate', 
            'cvv', 'cardName'
        ];

        let isValid = true;
        let firstInvalidField = null;

        requiredFields.forEach(fieldId => {
            const field = $(`#${fieldId}`);
            const value = field.val().trim();
            
            if (!value) {
                field.addClass('is-invalid');
                isValid = false;
                if (!firstInvalidField) {
                    firstInvalidField = field;
                }
            } else {
                field.removeClass('is-invalid');
            }
        });

        // Validate email format
        const email = $('#email').val();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (email && !emailRegex.test(email)) {
            $('#email').addClass('is-invalid');
            isValid = false;
            if (!firstInvalidField) {
                firstInvalidField = $('#email');
            }
        }

        // Validate card number (basic validation)
        const cardNumber = $('#cardNumber').val().replace(/\s/g, '');
        if (cardNumber && cardNumber.length < 13) {
            $('#cardNumber').addClass('is-invalid');
            isValid = false;
            if (!firstInvalidField) {
                firstInvalidField = $('#cardNumber');
            }
        }

        // Validate terms checkbox
        if (!$('#termsCheck').is(':checked')) {
            $('#termsCheck').addClass('is-invalid');
            isValid = false;
            if (!firstInvalidField) {
                firstInvalidField = $('#termsCheck');
            }
        } else {
            $('#termsCheck').removeClass('is-invalid');
        }

        if (!isValid && firstInvalidField) {
            firstInvalidField.focus();
            this.showError('Please fill in all required fields correctly.');
        }

        return isValid;
    }

    processOrder() {
        if (!this.validateCart()) {
            return;
        }

        if (!this.validateForm()) {
            return;
        }

        // Show loading state
        const btn = $('#place-order-btn');
        const originalText = btn.html();
        btn.prop('disabled', true).html('<i class="fa fa-spinner fa-spin me-2"></i>Processing...');

        // Simulate order processing
        setTimeout(() => {
            this.completeOrder();
            btn.prop('disabled', false).html(originalText);
        }, 2000);
    }

    completeOrder() {
        // Create order object
        const order = {
            id: this.generateOrderId(),
            date: new Date().toISOString(),
            customer: {
                firstName: $('#firstName').val(),
                lastName: $('#lastName').val(),
                email: $('#email').val(),
                phone: $('#phone').val(),
                address: $('#address').val(),
                city: $('#city').val(),
                state: $('#state').val(),
                zipCode: $('#zipCode').val()
            },
            items: this.cart,
            subtotal: parseFloat($('#subtotal').text().replace('$', '')),
            shipping: this.shippingCost,
            tax: parseFloat($('#tax').text().replace('$', '')),
            total: parseFloat($('#total').text().replace('$', '')),
            notes: $('#orderNotes').val()
        };

        // Save order to localStorage (in a real app, this would be sent to a server)
        const orders = JSON.parse(localStorage.getItem('teaOrders')) || [];
        orders.push(order);
        localStorage.setItem('teaOrders', JSON.stringify(orders));

        // Clear cart
        localStorage.removeItem('teaCart');
        this.cart = [];

        // Show success message and redirect
        this.showSuccess('Order placed successfully! You will receive a confirmation email shortly.');
        
        setTimeout(() => {
            window.location.href = 'order-confirmation.html?orderId=' + order.id;
        }, 2000);
    }

    generateOrderId() {
        const timestamp = Date.now().toString();
        const random = Math.random().toString(36).substring(2, 8);
        return `TH-${timestamp}-${random}`.toUpperCase();
    }

    showSuccess(message) {
        const notification = $(`
            <div class="alert alert-success alert-dismissible fade show position-fixed"
                 style="top: 20px; right: 20px; z-index: 9999; min-width: 300px;">
                <i class="fa fa-check-circle me-2"></i>
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `);
        
        $('body').append(notification);
        
        setTimeout(() => {
            notification.alert('close');
        }, 5000);
    }

    showError(message) {
        const notification = $(`
            <div class="alert alert-danger alert-dismissible fade show position-fixed"
                 style="top: 20px; right: 20px; z-index: 9999; min-width: 300px;">
                <i class="fa fa-exclamation-circle me-2"></i>
                ${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `);
        
        $('body').append(notification);
        
        setTimeout(() => {
            notification.alert('close');
        }, 5000);
    }
}

// Initialize checkout when document is ready
$(document).ready(function() {
    window.checkout = new Checkout();
}); 