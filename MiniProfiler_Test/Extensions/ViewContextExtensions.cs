using Microsoft.AspNetCore.Mvc.Rendering;

namespace MiniProfiler_Test.Extensions;

public static class ViewContextExtensions
{
    public static bool IsMenuItemActive(this ViewContext viewContext, string actions,
        string controller = "Dashboard", string area = "UserPanel")
    {
        var currentAction = viewContext.RouteData.Values["action"]?.ToString();
        var currentController = viewContext.RouteData.Values["controller"]?.ToString();
        var currentArea = viewContext.RouteData.Values["area"]?.ToString();

        return currentArea == area &&
               currentController == controller &&
               actions.Split(',').Contains(currentAction, StringComparer.OrdinalIgnoreCase);
    }
}