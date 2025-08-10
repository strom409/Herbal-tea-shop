// Order Confirmation Functionality
class OrderConfirmation {
    constructor() {
        this.orderId = this.getOrderIdFromUrl();
        this.init();
    }

    init() {
        if (this.orderId) {
            this.displayOrderDetails();
        } else {
            this.showError('No order ID found.');
        }
    }

    getOrderIdFromUrl() {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get('orderId');
    }

    displayOrderDetails() {
        const orders = JSON.parse(localStorage.getItem('teaOrders')) || [];
        const order = orders.find(o => o.id === this.orderId);

        if (!order) {
            this.showError('Order not found.');
            return;
        }

        const orderDetails = $('#order-details');
        const orderDate = new Date(order.date).toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });

        let detailsHTML = `
            <div class="row text-start">
                <div class="col-md-6 mb-3">
                    <h6 class="text-primary mb-2">Order Information</h6>
                    <p class="mb-1"><strong>Order ID:</strong> ${order.id}</p>
                    <p class="mb-1"><strong>Date:</strong> ${orderDate}</p>
                    <p class="mb-1"><strong>Status:</strong> <span class="badge bg-success">Confirmed</span></p>
                </div>
                <div class="col-md-6 mb-3">
                    <h6 class="text-primary mb-2">Customer Information</h6>
                    <p class="mb-1"><strong>Name:</strong> ${order.customer.firstName} ${order.customer.lastName}</p>
                    <p class="mb-1"><strong>Email:</strong> ${order.customer.email}</p>
                    <p class="mb-1"><strong>Phone:</strong> ${order.customer.phone}</p>
                </div>
            </div>
            
            <div class="row text-start">
                <div class="col-12 mb-3">
                    <h6 class="text-primary mb-2">Shipping Address</h6>
                    <p class="mb-1">${order.customer.address}</p>
                    <p class="mb-1">${order.customer.city}, ${order.customer.state} ${order.customer.zipCode}</p>
                </div>
            </div>

            <div class="row text-start">
                <div class="col-12 mb-3">
                    <h6 class="text-primary mb-2">Order Items</h6>
                    <div class="table-responsive">
                        <table class="table table-sm">
                            <thead>
                                <tr>
                                    <th>Item</th>
                                    <th>Quantity</th>
                                    <th>Price</th>
                                    <th>Total</th>
                                </tr>
                            </thead>
                            <tbody>
        `;

        order.items.forEach(item => {
            const itemTotal = item.price * item.quantity;
            detailsHTML += `
                <tr>
                    <td>
                        <div class="d-flex align-items-center">
                            <img src="${item.image}" alt="${item.name}" 
                                 style="width: 30px; height: 30px; object-fit: cover;" class="me-2">
                            <span>${item.name}</span>
                        </div>
                    </td>
                    <td>${item.quantity}</td>
                    <td>$${item.price.toFixed(2)}</td>
                    <td>$${itemTotal.toFixed(2)}</td>
                </tr>
            `;
        });

        detailsHTML += `
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div class="row text-start">
                <div class="col-md-6 offset-md-6">
                    <div class="border-top pt-3">
                        <div class="d-flex justify-content-between mb-2">
                            <span>Subtotal:</span>
                            <span>$${order.subtotal.toFixed(2)}</span>
                        </div>
                        <div class="d-flex justify-content-between mb-2">
                            <span>Shipping:</span>
                            <span>$${order.shipping.toFixed(2)}</span>
                        </div>
                        <div class="d-flex justify-content-between mb-2">
                            <span>Tax:</span>
                            <span>$${order.tax.toFixed(2)}</span>
                        </div>
                        <div class="d-flex justify-content-between mb-3">
                            <strong>Total:</strong>
                            <strong>$${order.total.toFixed(2)}</strong>
                        </div>
                    </div>
                </div>
            </div>
        `;

        if (order.notes) {
            detailsHTML += `
                <div class="row text-start">
                    <div class="col-12 mb-3">
                        <h6 class="text-primary mb-2">Order Notes</h6>
                        <p class="mb-0">${order.notes}</p>
                    </div>
                </div>
            `;
        }

        orderDetails.html(detailsHTML);
    }

    showError(message) {
        const orderDetails = $('#order-details');
        orderDetails.html(`
            <div class="alert alert-danger">
                <i class="fa fa-exclamation-circle me-2"></i>
                ${message}
            </div>
        `);
    }
}

// Initialize confirmation when document is ready
$(document).ready(function() {
    window.orderConfirmation = new OrderConfirmation();
}); 