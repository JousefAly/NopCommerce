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
            PublishableKey = "pk_test_51QlpoxGIYp5gF2nJgK4rwrgW2aDiIinlBhiYhtgdnpD6M8vouHsExr3s81pWQgGECXjWCMSBZtimMwa9LwKMbz7H00lMkvmA1n",
            ClientSecret = "sk_test_51QlpoxGIYp5gF2nJhM5rQ7sKwWsh8nGVQsxlLPy8esDRUMchXb9KnaoQcAALQFwyXRY8miaAa8bQkBo9BcFXsPkT00nWZMefXS",
            Amount = 5300
        };

        return View("~/Plugins/Payments.StripeCustom/Views/PaymentInfo.cshtml", model);
    }
}