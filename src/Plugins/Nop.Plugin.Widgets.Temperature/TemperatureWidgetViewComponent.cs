using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        double? temperature = await GetCurrentTemperatureAsync();
        temperature -= 273.15; //convert kelvin to celsius
        var model = new TemperatureModel
        {
            Value = (double?)(int?)temperature ?? 21
        };
        return View("~/Plugins/Widgets.Temperature/Views/PublicInfo.cshtml", model);
    }

    private async Task<double?> GetCurrentTemperatureAsync()
    {
        string apiKey = "36c9ac984bc4c2c4d826f5ddf66ad23e";
        HttpClient httpClient = new HttpClient();        
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat=30.04&lon=31.14&appid={apiKey}";

        HttpResponseMessage response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);
            return (double?)data["main"]?["temp"];
        }
        return null;
    }
}
