namespace ExportUserList
{
    public class PluginConstants
    {
        /// <summary>
        /// This command needs to be duplicated when more than one Command is supported
        /// </summary>
        public class Command
        {
            public const string ContentMenuId = "export_user_list_cm"; //Should match in CSS
            public const string Name = "ExportUserListToCsv"; // Should match Alchemy.command in ...Command.js
            public const string RibbonButtonId = "ExportUserListToCsvButton"; //Should match in CSS

            public const string RibbonName = Plugin.WebstoreName;
            public const string RibbonToolTip = Plugin.WebstoreName;
        }

        public class Plugin
        {
            public const string Name = "ExportUserList";
            public const string WebstoreDescription = "Adds a new Export User List option to the ribbon and tool bar, allowing to download the userlist as CSV.";
            public const string WebstoreName = "Export User List";
        }
    }
}