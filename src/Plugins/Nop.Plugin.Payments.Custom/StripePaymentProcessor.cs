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
            // Will need to see how
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
                            UnitAmount = (long)(processPaymentRequest.OrderTotal * 100),
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
                SuccessUrl = "https://localhost:44369/checkout/completed/",
                CancelUrl = "https://localhost:44369/"
            };

            var service = new SessionService();
            Session session = service.Create(options);


            processPaymentRequest.CustomValues.Add("checkoutUrl", session.Url);

            return new ProcessPaymentResult
            {
                NewPaymentStatus = PaymentStatus.Pending,

            };
        }      
    }
}