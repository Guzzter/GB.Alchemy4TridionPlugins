namespace UsageReport.Controllers
{
    public class ItemUsageDetails
    {
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets the TCM identifier.
        /// </summary>
        /// <value>The TCM identifier.</value>
        public string TcmId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the used count.
        /// </summary>
        /// <value>The used count.</value>
        public int UsedCount { get; set; }

        /// <summary>
        /// Gets or sets the web dav location.
        /// </summary>
        /// <value>The web dav location.</value>
        public string WebDavLocation { get; set; }
    }
}