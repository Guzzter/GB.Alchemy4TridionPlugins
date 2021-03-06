﻿namespace SaveCloseAndPublish.GUI
{
    public class PluginResourceGroup : Alchemy4Tridion.Plugins.GUI.Configuration.ResourceGroup
    {
        public PluginResourceGroup()
        {
            // Only the filename of our JS files are needed
            this.AddFile(PluginConstants.Command.CmdName + "Command.js");

            // Only the filename of our CSS files are needed
            this.AddFile(PluginConstants.Name + ".css");

            // Add generic type param to reference our command set
            this.AddFile<PluginCommandSet>();

            // Adds the web api proxy JS to this resource group... this allows us to call our webapi service without any 3rd party libs.
            this.AddWebApiProxy();
        }
    }
}