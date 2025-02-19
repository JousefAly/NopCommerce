using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.Custom.Components;
using Nop.Plugin.Payments.Stripe;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Stripe;

namespace Nop.Plugin.Payments.Custom;

public class CustomPaymentPlugin : BasePlugin, IPaymentMethod
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomPaymentPlugin(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    /// <summary>
    /// Gets a value indicating whether capture is supported
    /// </summary>
    public bool SupportCapture => false;

    /// <summary>
    /// Gets a value indicating whether void is supported
    /// </summary>
    public bool SupportVoid => false;

    /// <summary>
    /// Gets a value indicating whether refund is supported
    /// </summary>
    public bool SupportRefund => false;

    /// <summary>
    /// Gets a value indicating whether partial refund is supported
    /// </summary>
    public bool SupportPartiallyRefund => false;

    /// <summary>
    /// Gets a recurring payment type of payment method
    /// </summary>
    public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

    /// <summary>
    /// Gets a payment method type
    /// </summary>
    public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;

    /// <summary>
    /// Gets a value indicating whether we should display a payment information page for this plugin
    /// </summary>
    public bool SkipPaymentInfo => false;

    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => false;


    public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
    {
        return Task.FromResult(new CancelRecurringPaymentResult { Errors = new[] { "Recurring payment not supported" } });
    }

    public Task<bool> CanRePostProcessPaymentAsync(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return Task.FromResult(false);
    }

    public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
    {
        return Task.FromResult(new CapturePaymentResult { Errors = new[] { "capture payment not supported" } });
    }

    public Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
    {
        return Task.FromResult(155.5m);
    }

    public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
    {
        return Task.FromResult(new ProcessPaymentRequest());
    }

    public Task<string> GetPaymentMethodDescriptionAsync()
    {
        return Task.FromResult("custom payment discription");
    }

    public Type GetPublicViewComponent()
    {
        return typeof(PaymentInfoViewComponent);
    }

    public async Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
    {
        return await Task.FromResult(false);
    }

    public override Task InstallAsync()
    {
        Console.WriteLine("installing custom payment plugin");
        return base.InstallAsync();
    }

    public Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
    {



        return Task.CompletedTask;
    }

    public Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
    {

        //do some stuff here to complete the payment

        //long amount = (long)(processPaymentRequest.OrderTotal * 100);

        //StripeConfiguration.ApiKey = "sk_test_51QlpoxGIYp5gF2nJhM5rQ7sKwWsh8nGVQsxlLPy8esDRUMchXb9KnaoQcAALQFwyXRY8miaAa8bQkBo9BcFXsPkT00nWZMefXS";

        //var amountInCents = (long)(amount * 100);

        //var options = new PaymentIntentCreateOptions
        //{
        //    Amount = amountInCents,
        //    Currency = "usd",
        //    PaymentMethodTypes = new List<string> { "card" }
        //};

        //var service = new PaymentIntentService();
        //var paymentIntent = service.Create(options);


        //inject as a service
        var stripePaymentProcessor = new StripePaymentProcessor();

        stripePaymentProcessor.ProcessPayment(processPaymentRequest);
        string checkoutUrl = (string)processPaymentRequest.CustomValues["checkoutUrl"];

        //try to figure out redirect here

        _httpContextAccessor.HttpContext.Request..Redirect(checkoutUrl);
        Console.WriteLine("Order with total amount: " + processPaymentRequest.OrderTotal + "is paid to an external payment gateway.");
        return Task.FromResult(new ProcessPaymentResult());
    }

    public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
    {
        return Task.FromResult(new ProcessPaymentResult { Errors = new[] { "Recurring payment not supported" } });
    }

    public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
    {
        return Task.FromResult(new RefundPaymentResult { Errors = new[] { "Capture method not supported" } });
    }

    public override Task UninstallAsync()
    {
        return base.UninstallAsync();
    }

    public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
    {
        return Task.FromResult<IList<string>>(new List<string>());
    }

    public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
    {
        return Task.FromResult(new VoidPaymentResult { Errors = new[] { "void payment not supported" } });
    }
}
