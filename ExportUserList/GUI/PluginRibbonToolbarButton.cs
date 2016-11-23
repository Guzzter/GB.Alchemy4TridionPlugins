using Alchemy4Tridion.Plugins.GUI.Configuration;

namespace ExportUserList.GUI
{
    public class PluginRibbonToolbarButton : RibbonToolbarExtension
    {
        public PluginRibbonToolbarButton()
        {
            // The unique identifier used for the html element created.
            AssignId = PluginConstants.Command.RibbonButtonId;

            // The name of the command to execute when clicked
            Command = PluginConstants.Command.Name;

            // The label of the button.
            Name = PluginConstants.Command.RibbonName;

            // The page tab to assign this extension to. See Constants.PageIds.
            PageId = Constants.PageIds.AdministrationPage;

            // Option GroupId, put this into an existing group (not capable if using a .ascx Control)
            //GroupId = Constants.GroupIds.HomePage.ShareGroup;
            GroupId = @"AccessManagementGroup";
            InsertBefore = @"UsersDropdown";

            // The tooltip label that will get applied.
            Title = PluginConstants.Command.RibbonToolTip;

            // Add a dependency to the resource group that contains the files/commands that this toolbar extension will use.
            Dependencies.Add<PluginResourceGroup>();

            // apply the extension to a specific view.
            Apply.ToView(Constants.Views.DashboardView, "DashboardToolbar");

            PageId = Constants.PageIds.AdministrationPage;
        }
    }
}