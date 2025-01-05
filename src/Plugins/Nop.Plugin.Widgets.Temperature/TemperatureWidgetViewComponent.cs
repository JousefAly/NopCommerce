using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.Temperature.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.Temperature;
[ViewComponent(Name = "TemperatureWidget")]
public class TemperatureWidgetViewComponent : NopViewComponent
{
    public TemperatureWidgetViewComponent()
    {

    }
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var model = new TemperatureModel
        {
            Value = 19.5
        };
        return Content($"Temperature is: {model.Value}");
    }
}
