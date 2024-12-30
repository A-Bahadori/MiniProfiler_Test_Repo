namespace MiniProfiler_Test.Areas.Admin.Models;

public class DashboardSidebarVM
{
    public string UserName { get; set; }
    public List<SidebarMenuItem> MenuItems { get; set; }
}

public class SidebarMenuItem
{
    public string Title { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
    public bool IsActive { get; set; }
}