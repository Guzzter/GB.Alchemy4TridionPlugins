namespace ExportUserList
{
    public class PluginConstants
    {
        public const string Name = "ExportUserList";

        public const string WebstoreDescription =
                        "Adds a new Export User List option to the ribbon and tool bar, allowing to download the userlist as CSV.";

        public const string WebstoreName = "Export User List";

        /// <summary>
        /// This command needs to be duplicated when more than one Command is supported
        /// </summary>
        public class Command
        {
            public const string CmdName = "ExportUserListToCsv"; //Should match the Javascript filename
            public const string ContentMenuId = "cm_export_user_list"; //Should match in CSS

            // Should match Alchemy.command in ...Command.js

            public const string RibbonButtonId = "ExportUserListToCsvButton"; //Should match in CSS

            public const string RibbonName = WebstoreName;

            public const string RibbonToolTip = WebstoreName;
        }
    }
}