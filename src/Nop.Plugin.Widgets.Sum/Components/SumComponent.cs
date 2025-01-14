using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.Sum.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Sum.Components;

[ViewComponent(Name = "Sum")]
public class SumComponent : NopViewComponent
{

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new SumCalculatorModel();

        return View("~/Plugins/Widgets.Sum/Views/Calculate.cshtml", model);
    }
}
