using Microsoft.AspNetCore.Mvc;

namespace MiniProfiler_Test.Controllers;

public class SiteBaseController : Controller
{
    protected string ErrorMessage = "ErrorMessage";
    protected string InfoMessage = "InfoMessage";
    protected string SuccessMessage = "SuccessMessage";
    protected string WarningMessage = "WarningMessage";
}