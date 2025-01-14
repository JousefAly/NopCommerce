using Nop.Plugin.Widgets.Sum.Components;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.Sum;

public class SumPlugin : BasePlugin, IWidgetPlugin
{
    public bool HideInWidgetList => false;

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(SumComponent);
    }

    public async Task<IList<string>> GetWidgetZonesAsync()
    {
        return new List<string> { PublicWidgetZones.HeaderAfter };
    }

}
