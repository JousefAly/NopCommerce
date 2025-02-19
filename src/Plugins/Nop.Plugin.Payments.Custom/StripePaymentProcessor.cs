using Nop.Core.Domain.Payments;
using Nop.Services.Payments;
using Stripe;
using Stripe.Checkout;

namespace Nop.Plugin.Payments.Stripe
{
    public class StripePaymentProcessor
    {
        private readonly string _stripeSecretKey;

        public StripePaymentProcessor()
        {
            // Load from plugin settings
            _stripeSecretKey = "sk_test_51QlpoxGIYp5gF2nJhM5rQ7sKwWsh8nGVQsxlLPy8esDRUMchXb9KnaoQcAALQFwyXRY8miaAa8bQkBo9BcFXsPkT00nWZMefXS"; 
            StripeConfiguration.ApiKey = _stripeSecretKey;
        }

        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
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
                            UnitAmount = (long)(processPaymentRequest.OrderTotal * 100), // Convert to cents
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Order Payment",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "https://yourdomain.com/checkout/completed", // Redirect after success
                CancelUrl = "https://yourdomain.com/checkout/cancel",  // Redirect after cancel
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return new ProcessPaymentResult
            {
                NewPaymentStatus = PaymentStatus.Pending,
                //RedirectUrl = session.Url // Redirect to Stripe checkout
            };
        }

        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            // Handle post-payment logic (e.g., update order status)
        }
    }
}