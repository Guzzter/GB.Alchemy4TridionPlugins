using Alchemy4Tridion.Plugins.GUI.Configuration;

namespace ExportUserList.GUI
{
    public class PluginContextMenuExtension : Alchemy4Tridion.Plugins.GUI.Configuration.ContextMenuExtension
    {
        public PluginContextMenuExtension()
        {
            AssignId = "";

            // Use this property to specify where in the context menu your items will go
            InsertBefore = Constants.ContextMenuIds.MainContextMenu.SendItemLink;

            // Use AddItem() or AddSubMenu() to add items for this context menu

            // element id title command name
            AddItem(PluginConstants.Command.ContentMenuId, PluginConstants.Plugin.WebstoreName, PluginConstants.Command.Name);

            // Add a dependency to the resource group that contains the files/commands that this toolbar extension will use.
            Dependencies.Add<PluginResourceGroup>();

            // apply the extension to a specific view.
            Apply.ToView(Constants.Views.DashboardView);
        }
    }
}