namespace UsageReport
{
    public class PluginConstants
    {
        public const string Name = "UsageReport";

        public const string WebstoreDescription = "Adds a new Usage Report option to the ribbon and tool bar, showing the usage count for each item in a folder.";

        public const string WebstoreName = "Usage Report";

        /// <summary>
        /// This command needs to be duplicated when more than one Command is supported
        /// </summary>
        public class Command
        {
            public const string CmdName = "UsageReport";
            public const string ContentMenuId = "cm_usagereport"; //Should match in CSS

            // Should match Alchemy.command in ...Command.js
            public const string RibbonButtonId = "UsageReportButton"; //Should match in CSS

            public const string RibbonName = "Usage Report";
            public const string RibbonToolTip = WebstoreName;
        }
    }
}