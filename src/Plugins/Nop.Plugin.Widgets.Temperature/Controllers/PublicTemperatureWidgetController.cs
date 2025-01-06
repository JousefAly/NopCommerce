using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.Widgets.Temperature.Models;

namespace Nop.Plugin.Widgets.Temperature.Controllers;
[AutoValidateAntiforgeryToken]
public class PublicTemperatureWidgetController : BasePluginController
{
    private readonly IPermissionService _permissionService;

    public PublicTemperatureWidgetController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
    public async Task<IActionResult> Configure()
    {
        return View("~/Plugins/Widgets.Temperature/Views/PublicInfo.cshtml");
    }

    //[HttpPost]
    //public async Task<IActionResult> Configure(ConfigurationModel model)
    //{
    //    return await Configure();
    //}
}
