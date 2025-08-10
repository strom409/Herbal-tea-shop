using Stripe;
using Stripe.Checkout;

namespace MvcHer.Services
{
    public interface IStripeService
    {
        Task<Session> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl, string cancelUrl);
        Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId);
    }

    public class StripeService : IStripeService
    {
        private readonly string _secretKey;
        private readonly string _publishableKey;

        public StripeService(IConfiguration configuration)
        {
            _secretKey = configuration["Stripe:SecretKey"] ?? throw new ArgumentNullException("Stripe:SecretKey");
            _publishableKey = configuration["Stripe:PublishableKey"] ?? throw new ArgumentNullException("Stripe:PublishableKey");
            
            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<Session> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl, string cancelUrl)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(amount * 100), // Convert to cents
                                Currency = currency.ToLower(),
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Herbal Tea Order",
                                    Description = "Your tea order from Herbal Tea Store"
                                }
                            },
                            Quantity = 1
                        }
                    },
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl,
                    PaymentIntentData = new SessionPaymentIntentDataOptions
                    {
                        CaptureMethod = "automatic"
                    }
                };

                var service = new SessionService();
                return await service.CreateAsync(options);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Stripe checkout session: {ex.Message}", ex);
            }
        }

        public async Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                return await service.GetAsync(paymentIntentId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving payment intent: {ex.Message}", ex);
            }
        }

        public string GetPublishableKey()
        {
            return _publishableKey;
        }
    }
}
