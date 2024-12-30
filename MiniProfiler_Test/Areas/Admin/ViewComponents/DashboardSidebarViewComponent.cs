using Microsoft.AspNetCore.Mvc;
using MiniProfiler_Test.Areas.Admin.Models;
using MiniProfiler_Test.Extensions;

namespace MiniProfiler_Test.Areas.Admin.ViewComponents;

public class DashboardSidebarViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new DashboardSidebarVM()
        {
            MenuItems = new List<SidebarMenuItem>
            {
                new SidebarMenuItem
                {
                    Title = "داشبورد",
                    Icon = "fas fa-home",
                    Url = Url.Action("Index", "Home", new { area = "Admin" }),
                    IsActive = ViewContext.IsMenuItemActive("Index","Home","Admin")
                },
                new SidebarMenuItem
                {
                    Title = "مدیریت کاربران",
                    Icon = "fas fa-users-cog",
                    Url = Url.Action("Index", "Users", new { area = "Admin" }),
                    IsActive = ViewContext.IsMenuItemActive("Index" ,"Users","Admin")
                }
            }
        };

        return View(model);
    }
}