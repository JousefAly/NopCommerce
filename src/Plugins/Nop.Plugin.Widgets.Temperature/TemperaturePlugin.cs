using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.Temperature;

public class TemperaturePlugin : BasePlugin, IWidgetPlugin
{
    public bool HideInWidgetList => false;

    public Type GetWidgetViewComponent(string widgetZone)   
    {
        return typeof(TemperatureWidgetViewComponent);
    }

    public async Task<IList<string>> GetWidgetZonesAsync()
    {
        return new List<string> { PublicWidgetZones.HeaderAfter };
    }
}
