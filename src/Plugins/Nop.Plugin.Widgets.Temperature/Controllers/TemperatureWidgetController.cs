using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.Widgets.Temperature.Models;
using Nop.Core;

namespace Nop.Plugin.Widgets.Temperature.Controllers;
[AuthorizeAdmin]
[Area(AreaNames.ADMIN)]
[AutoValidateAntiforgeryToken]
public class TemperatureWidgetController : BasePluginController
{
    private readonly IPermissionService _permissionService;

    public TemperatureWidgetController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
    public async Task<IActionResult> Configure()
    {
        return View("~/Plugins/Widgets.Temperature/Views/Configure.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        return await Configure();
    }
}
