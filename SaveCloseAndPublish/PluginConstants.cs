namespace SaveCloseAndPublish
{
    public class PluginConstants
    {
        public const string Name = "SaveCloseAndPublish";

        public const string WebstoreDescription = "When editing a Page directly perform 3 actions in 1 go: save, close and publish.";

        public const string WebstoreName = "Save Close And Publish";

        /// <summary>
        /// This command needs to be duplicated when more than one Command is supported
        /// </summary>
        public class Command
        {
            public const string CmdName = "SaveCloseAndPublish";
            public const string ContentMenuId = "save_close_and_publish_cm"; //Should match in CSS

            // Should match Alchemy.command in ...Command.js
            public const string RibbonButtonId = "SaveCloseAndPublishButton"; //Should match in CSS

            public const string RibbonName = WebstoreName;
            public const string RibbonToolTip = WebstoreName;
        }
    }
}