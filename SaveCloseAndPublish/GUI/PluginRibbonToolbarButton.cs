namespace SaveCloseAndPublish.GUI
{
    using Alchemy4Tridion.Plugins.GUI.Configuration;

    public class PluginRibbonToolbarButton : RibbonToolbarExtension
    {
        public PluginRibbonToolbarButton()
        {
            // The unique identifier used for the html element created.
            this.AssignId = PluginConstants.Command.RibbonButtonId;

            // The name of the command to execute when clicked
            this.Command = PluginConstants.Command.CmdName;

            // The label of the button.
            this.Name = PluginConstants.Command.RibbonName;

            // The page tab to assign this extension to. See Constants.PageIds.
            this.PageId = Constants.PageIds.AdministrationPage;

            // Option GroupId, put this into an existing group (not capable if using a .ascx Control)
            this.GroupId = Constants.GroupIds.AdministrationPage.AccessManagementGroup;
            this.GroupId = @"AccessManagementGroup";
            this.InsertBefore = @"UsersDropdown";

            // The tooltip label that will get applied.
            this.Title = PluginConstants.Command.RibbonToolTip;

            // Add a dependency to the resource group that contains the files/commands that this toolbar extension will use.
            this.Dependencies.Add<PluginResourceGroup>();

            // apply the extension to a specific view.
            this.Apply.ToView(Constants.Views.DashboardView, "DashboardToolbar");

            this.PageId = Constants.PageIds.AdministrationPage;
        }
    }
}