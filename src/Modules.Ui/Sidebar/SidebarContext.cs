namespace Modules.Ui.Sidebar;

public class SidebarContext
{
    public string? CurrentPath { get; set; } = string.Empty;
    public List<string> CurrentOpenDetails { get; set; } = [];
}
