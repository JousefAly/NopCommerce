using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Orders;
using Stripe;
using Stripe.Checkout;

namespace Nop.Plugin.Payments.PayPalCommerce.Controllers;

[Route("api/stripe")]
[ApiController]
public class StripeWebhookController : Controller
{
    private readonly string signingSecret = "whsec_66dff8d2ae4fcf308a2ce3441858a9f7b67f0ef1bfee7de6d64f3cc7f0a7ec5b"; //appsettings
    private readonly IOrderService _orderService;

    public StripeWebhookController(IOrderService orderService)
    {
        _orderService = orderService;
    }


    [HttpPost("webhook")]
    public async Task<IActionResult> HandleWebhook()
    {


        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
              json,
              Request.Headers["Stripe-Signature"],
              signingSecret
            );


            if (
              stripeEvent.Type == EventTypes.CheckoutSessionCompleted ||
              stripeEvent.Type == EventTypes.CheckoutSessionAsyncPaymentSucceeded
            )
            {
                var session = stripeEvent.Data.Object as Session;

                await FulfillCheckout(session.Id);
            }

            return Ok();
        }
        catch (StripeException)
        {
            return BadRequest();
        }


    }
    public async Task FulfillCheckout(string sessionId)
    {
        // Set your secret key. Remember to switch to your live secret key in production.
        // See your keys here: https://dashboard.stripe.com/apikeys
        StripeConfiguration.ApiKey = "sk_test_51QlpoxGIYp5gF2nJhM5rQ7sKwWsh8nGVQsxlLPy8esDRUMchXb9KnaoQcAALQFwyXRY8miaAa8bQkBo9BcFXsPkT00nWZMefXS";

        Console.WriteLine("Fulfilling Checkout Session " + sessionId);

        // TODO: Make this function safe to run multiple times,
        // even concurrently, with the same session ID

        // TODO: Make sure fulfillment hasn't already been
        // peformed for this Checkout Session

        // Retrieve the Checkout Session from the API with line_items expanded
        var options = new SessionGetOptions
        {
            Expand = new List<string> { "line_items" },
        };

        var service = new SessionService();
        var checkoutSession = service.Get(sessionId, options);


        //int orderId = 16;
        checkoutSession.Metadata.TryGetValue("OrderId", out string orderIdStr);

        var order = await _orderService.GetOrderByGuidAsync(Guid.Parse(orderIdStr));

        if (order != null && order.OrderStatus == OrderStatus.Pending)
        {
            // Update order status to Complete
            order.PaymentStatus = PaymentStatus.Paid;
            order.PaidDateUtc = DateTime.UtcNow;

            await _orderService.UpdateOrderAsync(order);
        }


        if (checkoutSession.PaymentStatus != "unpaid")
        {
            Console.WriteLine($"Fulfilled successfully for session id {sessionId}");
        }
    }

}