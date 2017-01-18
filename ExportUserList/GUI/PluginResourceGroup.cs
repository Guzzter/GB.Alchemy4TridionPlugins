namespace ExportUserList.GUI
{
    public class PluginResourceGroup : Alchemy4Tridion.Plugins.GUI.Configuration.ResourceGroup
    {
        public PluginResourceGroup()
        {
            // only the filename of our JS files are needed
            this.AddFile(PluginConstants.Command.CmdName + "Command.js");

            // only the filename of our CSS files are needed
            this.AddFile(PluginConstants.Name + ".css");

            // add genertic type param to reference our command set
            this.AddFile<PluginCommandSet>();

            // Adds the web api proxy JS to this resource group... this allows us to call our webapi service without any 3rd party libs.
            this.AddWebApiProxy();

            // AddWebApiProxy() includes Alchemy.Core as a dependency already... if not using the proxy you can remove the comment from below.

            // Dependencies.AddAlchemyCore();
        }
    }
}