using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.SumNumbers.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.SumNumbers.Components;

[ViewComponent(Name = "SumNumbers")]
public class SumComponent : NopViewComponent
{

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new SumCalculatorModel();

        return View("~/Plugins/Widgets.SumNumbers/Views/SumNumbers.cshtml", model);
    }
}
