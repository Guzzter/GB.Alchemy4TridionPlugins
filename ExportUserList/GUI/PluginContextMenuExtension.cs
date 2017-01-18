namespace ExportUserList.GUI
{
    using Alchemy4Tridion.Plugins.GUI.Configuration;

    using ExportUserList;

    public class PluginContextMenuExtension : Alchemy4Tridion.Plugins.GUI.Configuration.ContextMenuExtension
    {
        public PluginContextMenuExtension()
        {
            this.AssignId = PluginConstants.Name;

            // Use this property to specify where in the context menu your items will go
            this.InsertBefore = Constants.ContextMenuIds.MainContextMenu.SendItemLink;

            // Use AddItem() or AddSubMenu() to add items for this context menu

            // element id title command name
            this.AddItem(PluginConstants.Command.ContentMenuId, PluginConstants.WebstoreName, PluginConstants.Command.CmdName);

            // Add a dependency to the resource group that contains the files/commands that this toolbar extension will use.
            this.Dependencies.Add<PluginResourceGroup>();

            // apply the extension to a specific view.
            this.Apply.ToView(Constants.Views.DashboardView);
        }
    }
}