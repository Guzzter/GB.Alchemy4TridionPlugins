namespace NavPanelResizer.GUI
{
    using Alchemy4Tridion.Plugins.GUI.Configuration;

    /// <summary>
    /// Represents an extension element in the editor configuration for creating a context menu extension.
    /// </summary>
    public class NavContextMenu : ContextMenuExtension
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NavContextMenu()
        {
            // This is the id which gets put on the html element for this menu (to target with css/js).
            this.AssignId = "ext_" + PluginConstants.Name;

            // The name of the extension menu
            this.Name = PluginConstants.Name;

            // Where to add the new menu in the current context menu.
            this.InsertBefore = "cm_blue";

            // Generate all of the context menu items...
            AddItem("cm_" + PluginConstants.Name, PluginConstants.WebstoreName, PluginConstants.Name);

            // We need to addd our resource group as a dependency to this extension
            this.Dependencies.Add<PluginResourceGroup>();
            this.Dependencies.AddLibraryJQuery();

            // Actually apply our extension to a particular view. You can have multiple.
            this.Apply.ToView(Constants.Views.DashboardView);
        }
    }
}