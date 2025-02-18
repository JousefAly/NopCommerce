using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.Custom.Models;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Payments;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.Custom.Components;
public class PaymentInfoViewComponent : NopViewComponent
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly IPaymentService _paymentService;
    protected readonly OrderSettings _orderSettings;

    #endregion

    #region Ctor

    public PaymentInfoViewComponent(ILocalizationService localizationService,
        INotificationService notificationService,
        IPaymentService paymentService,
        OrderSettings orderSettings)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _paymentService = paymentService;
        _orderSettings = orderSettings;
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new PaymentInfoModel
        {
            Amount = 3500
        };
        return View("~/Plugins/Payments.Custom/Views/PaymentInfo.cshtml", model);
    }


    #endregion
}
