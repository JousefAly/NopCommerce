using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Payments.StripeCustom;
using Nop.Services.Configuration;

public class StripePaymentViewComponent : ViewComponent
{
    private readonly StripeSettings _stripeSettings;

    public StripePaymentViewComponent(ISettingService settingService)
    {
        _stripeSettings = settingService.LoadSetting<StripeSettings>();
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new StripePaymentModel
        {
            PublishableKey = _stripeSettings.PublishableKey,
            ClientSecret = TempData["StripeClientSecret"]?.ToString()
        };

        return View("~/Plugins/Payments.StripeCustom/Views/PaymentInfo.cshtml", model);
    }
}