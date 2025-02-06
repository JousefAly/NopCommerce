using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;
using Stripe;

namespace Nop.Plugin.Payments.StripeCustom;

public class StripeCustomPaymentProcessor : BasePlugin, IPaymentMethod
{
    private readonly ICurrencyService _currencyService;
    private readonly IWorkContext _workContext;
    private readonly ISettingService _settingService;
    private readonly IOrderService _orderService;
    private readonly ILocalizationService _localizationService;
    private readonly StripeSettings _stripeSettings;

    public StripeCustomPaymentProcessor(
        ICurrencyService currencyService,
        IWorkContext workContext,
        ISettingService settingService,
        IOrderService orderService,
        ILocalizationService localizationService)
    {
        _currencyService = currencyService;
        _workContext = workContext;
        _settingService = settingService;
        _orderService = orderService;
        _localizationService = localizationService;
        _stripeSettings = _settingService.LoadSetting<StripeSettings>();
        StripeConfiguration.ApiKey = "pk_test_51QlpoxGIYp5gF2nJgK4rwrgW2aDiIinlBhiYhtgdnpD6M8vouHsExr3s81pWQgGECXjWCMSBZtimMwa9LwKMbz7H00lMkvmA1n";
    }

    #region Properties
    public bool SupportCapture => true;
    public bool SupportPartiallyRefund => true;
    public bool SupportRefund => true;
    public bool SupportVoid => true;
    public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;
    public PaymentMethodType PaymentMethodType => PaymentMethodType.Button;
    public bool SkipPaymentInfo => false;
    #endregion

    #region Methods
    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Payments.StripeCustom.Fields.SecretKey"] = "Secret Key",
            ["Plugins.Payments.StripeCustom.Fields.PublishableKey"] = "Publishable Key",
            ["Plugins.Payments.StripeCustom.Fields.WebhookSecret"] = "Webhook Secret",
            ["Plugins.Payments.StripeCustom.Fields.Use3DS"] = "Use 3D Secure"
        });

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _settingService.DeleteSettingAsync<StripeSettings>();
        await base.UninstallAsync();
    }

    public async Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest request)
    {
        return new CancelRecurringPaymentResult { Errors = new[] { "Recurring payments not supported" } };
    }

    public async Task<bool> CanRePostProcessPaymentAsync(Order order)
    {
        return false;
    }

    public async Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest request)
    {
        var result = new CapturePaymentResult();

        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.CaptureAsync(request.Order.AuthorizationTransactionId);

            result.NewPaymentStatus = PaymentStatus.Paid;
            result.CaptureTransactionId = paymentIntent.Id;
        }
        catch (StripeException ex)
        {
            result.AddError($"Stripe error: {ex.StripeError.Message}");
        }

        return result;
    }

    public async Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
    {
        return decimal.Zero;
    }

    public async Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
    {
        return new ProcessPaymentRequest();
    }

    public async Task<string> GetPaymentMethodDescriptionAsync()
    {
        return await _localizationService.GetResourceAsync("Plugins.Payments.StripeCustom.PaymentMethodDescription");
    }

    public Type GetPublicViewComponent()
    {
        return typeof(StripePaymentViewComponent);
    }

    public async Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
    {
        return false;
    }

    public async Task PostProcessPaymentAsync(PostProcessPaymentRequest request)
    {
        // Used for 3D Secure handling
    }

    public async Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        var result = new ProcessPaymentResult();
        var currency = await _workContext.GetWorkingCurrencyAsync();

        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(request.OrderTotal * 100),
                Currency = currency.CurrencyCode.ToLower(),
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string>
                {
                    { "OrderGuid", request.OrderGuid.ToString() }
                }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            result.NewPaymentStatus = PaymentStatus.Pending;
            result.AuthorizationTransactionId = paymentIntent.Id;
            result.AuthorizationTransactionResult = paymentIntent.Status;
            result.CaptureTransactionId = paymentIntent.Id;
            result.CaptureTransactionResult = paymentIntent.Status;
        }
        catch (StripeException ex)
        {
            result.AddError($"Stripe error: {ex.StripeError.Message}");
        }

        return result;
    }

    public async Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest request)
    {
        throw new NotImplementedException("Recurring payments not supported");
    }

    public async Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest request)
    {
        var result = new RefundPaymentResult();

        try
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = request.Order.AuthorizationTransactionId,
                Amount = request.IsPartialRefund
                    ? (long)(request.AmountToRefund * 100)
                    : null
            };

            var service = new RefundService();
            var refund = await service.CreateAsync(options);

            result.NewPaymentStatus = request.IsPartialRefund
                ? PaymentStatus.PartiallyRefunded
                : PaymentStatus.Refunded;
        }
        catch (StripeException ex)
        {
            result.AddError($"Stripe error: {ex.StripeError.Message}");
        }

        return result;
    }

    public async Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
    {
        return new List<string>();
    }

    public async Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest request)
    {
        var result = new VoidPaymentResult();

        try
        {
            var service = new PaymentIntentService();
            var paymentIntent = await service.CancelAsync(request.Order.AuthorizationTransactionId);

            result.NewPaymentStatus = PaymentStatus.Voided;
        }
        catch (StripeException ex)
        {
            result.AddError($"Stripe error: {ex.StripeError.Message}");
        }

        return result;
    }
    #endregion
}
