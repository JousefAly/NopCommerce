using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.Sum.Models;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.Sum.Controllers;
public class SumController : BasePluginController
{
    public ActionResult Calculate()
    {
        var model = new SumCalculatorModel();
        return View("~/Plugins/Widgets.Sum/Views/Calculate.cshtml", model);
    }
    
    [HttpPost]
    public ActionResult Calculate(SumCalculatorModel model)
    {
        if (ModelState.IsValid)
        {
            model.Result = model.Number1 + model.Number2;
        }

        return View("~/Plugins/Widgets.Sum/Views/Calculate.cshtml", model);
    }

}
