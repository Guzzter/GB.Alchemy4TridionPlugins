namespace NavPanelResizer.GUI
{
    using NavPanelResizer;

    public class PluginResourceGroup : Alchemy4Tridion.Plugins.GUI.Configuration.ResourceGroup
    {
        public PluginResourceGroup()
        {
            // only the filename of our JS files are needed
            this.AddFile(PluginConstants.Command.CmdName + "Command.js");

            // add generic type param to reference our command set
            this.AddFile<PluginCommandSet>();

            // Adds the web api proxy JS to this resource group... this allows us to call our webapi service without any 3rd party libs.
            this.AddWebApiProxy();
        }
    }
}