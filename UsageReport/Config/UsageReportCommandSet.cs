using Alchemy4Tridion.Plugins.GUI.Configuration;

namespace UsageReport.Config
{
    public class UsageReportCommandSet : CommandSet
    {
        public UsageReportCommandSet()
        {
            // we only need to add the name of our command
            this.AddCommand(PluginConstants.Command.CmdName);
        }
    }
}