using Alchemy4Tridion.Plugins.GUI.Configuration;

namespace Alchemy.Plugins.GoToOwningPub.Config
{
    /// <summary>
    /// Represents an extension element in the editor configuration for creating a context menu extension.
    /// </summary>
    public class GoToOwnPubContextMenu : ContextMenuExtension
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GoToOwnPubContextMenu()
        {
            // This is the id which gets put on the html element for this menu (to target with css/js).
            AssignId = "ext_GoToOwningPublication"; 
            // The name of the extension menu
            Name = "GoToOwningPublicationExtension";
            // Where to add the new menu in the current context menu.
            InsertBefore = "cm_blue";

            // Generate all of the context menu items...
            AddItem("cm_GoToOwningPublication", "Go To Owning Publication", "GoToOwningPub");

            // We need to addd our resource group as a dependency to this extension
            Dependencies.Add<GoToOwnPubResourceGroup>();

            // Actually apply our extension to a particular view.  You can have multiple.
            Apply.ToView(Constants.Views.DashboardView);
        }
    }
}
