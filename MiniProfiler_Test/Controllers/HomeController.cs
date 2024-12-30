using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniProfiler_Test.Models;

namespace MiniProfiler_Test.Controllers;

public class HomeController : SiteBaseController
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}