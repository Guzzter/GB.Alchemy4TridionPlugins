using Alchemy4Tridion.Plugins.GUI.Configuration;

namespace UsageReport.Config
{
    /// <summary>
    /// Represents an extension element in the editor configuration for creating a context menu extension.
    /// </summary>
    public class UsageReportContextMenu : ContextMenuExtension
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UsageReportContextMenu()
        {
            // This is the id which gets put on the html element for this menu (to target with css/js).
            this.AssignId = PluginConstants.Name;

            // The name of the extension menu
            this.Name = PluginConstants.Name;

            // Where to add the new menu in the current context menu.
            this.InsertBefore = "cm_refresh";

            // Generate all of the context menu items...
            this.AddItem(PluginConstants.Command.ContentMenuId, PluginConstants.WebstoreName, PluginConstants.Command.CmdName);

            // We need to addd our resource group as a dependency to this extension
            this.Dependencies.Add<UsageReportResourceGroup>();

            // Actually apply our extension to a particular view. You can have multiple.
            this.Apply.ToView(Constants.Views.DashboardView);
        }
    }
}