using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniProfiler_Test.Controllers;

namespace MiniProfiler_Test.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class HomeController : SiteBaseController
{
    public IActionResult Index()
    {
        return View();
    }
}