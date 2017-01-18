using System;
using System.Web.Http;
using System.Xml.Linq;

namespace UsageReport.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Alchemy4Tridion.Plugins;

    using Tridion.ContentManager.CoreService.Client;

    /// <summary>
    /// A WebAPI web service controller that can be consumed by your front end.
    /// </summary>
    /// <remarks>
    /// The following conditions apply: 1.) Must have AlchemyRoutePrefix attribute. You pass in the type of your AlchemyPlugin (the one that inherits
    /// AlchemyPluginBase). 2.) Must inherit AlchemyApiController. 3.) All Action methods must have an Http Verb attribute on it as well as a
    /// RouteAttribute (otherwise it won't generate a js proxy).
    /// </remarks>
    [AlchemyRoutePrefix("UsageReportService")]
    public class UsageReportServiceController : AlchemyApiController
    {
        /*

        // GET /Alchemy/Plugins/HelloExample/api/UsageReportService/PagesWhereUsed/tcm/title/depth
        /// <summary>
        /// Finds any pages on which a Tridion object are used
        /// </summary>
        /// <param name="tcm">The TCM ID of a Tridion object for which this function should find any pages on which it is being used</param>
        /// <param name="title">The Title of a Tridion object for which this function should find any pages on which it is being used</param>
        /// <param name="depth">
        /// The number of connections we should traverse looking for pages on which an object is used, ex from one component to another component onto
        /// which it is component linked This prevents infinite loops, such as a component A linked to component B which is linked back to component A
        /// </param>
        /// <returns>Formatted HTML containing the list of pages using our Tridion object</returns>
        [HttpGet]
        [Route("PagesWhereUsed/{tcm}/{title}/{depth}")]
        public string GetPagesWhereUsed(string tcm, string title, int depth)
        {
            SessionAwareCoreServiceClient client = null;
            try
            {
                // Creates a new core service client
                client = new SessionAwareCoreServiceClient("netTcp_2013");

                // Gets the current user so we can impersonate them for our client
                string username = GetUserName();
                client.Impersonate(username);

                // Create a new string to hold the HTML we will use to display the pages using this component
                string html = "";

                // Call a function to get the pages HTML
                html = GetPages("tcm:" + tcm, client, html, depth, 1);

                // If the HTML we get from our function is empty it means no page is using our Tridion object within the allowed number of links, so we
                // say so.
                if (String.IsNullOrEmpty(html))
                {
                    html = title + "is not used on any pages";
                }
                else
                {
                    // If it isn't empty it means we do have some pages, so we create a heading to describe the results and add a div around them
                    string heading = "<h2>" + title + " is used on the following pages:</h2>";
                    heading += "<div class=\"results\">";
                    heading += CreateItemsHeading();
                    html = heading + html + "</div>";
                }

                // Return the HTML representing the pages on which our Tridion object is used
                return html;
            }
            catch (Exception ex)
            {
                // proper way of ensuring that the client gets closed... we close it in our try block above, then in a catch block if an exception is
                // thrown we abort it.
                if (client != null)
                {
                    client.Abort();
                }

                // we are rethrowing the original exception and just letting webapi handle it
                throw ex;
            }
            finally
            {
                // We no longer need our core service client so we close it now to free resources
                if (client != null)
                {
                    client.Close();
                }
            }
        }
        */

        // GET /Alchemy/Plugins/HelloExample/api/UsageReportService/UsingAndUsedItems/tcm/title
        /// <summary>
        /// Finds the list of items being used by a given Tridion object and the list of items using that object.
        /// </summary>
        /// <param name="tcm">The TCM ID of a Tridion object for which this function should find the using and used items in Tridion</param>
        /// <param name="title">The Title of a Tridion object for which this function should find the using and used items in Tridion</param>
        /// <returns>Formatted HTML containing the lists of using and used items for the input Tridion object</returns>
        [HttpGet]
        [Route("UseCountList/{tcm}/{title}")]
        public string GetUseCountList(string tcm, string title)
        {
            try
            {
                // Creates a new OrganizationalItemItemsFilterData
                OrganizationalItemItemsFilterData itemItemsFilterData = new OrganizationalItemItemsFilterData();
                itemItemsFilterData.Recursive = false;
                /*itemItemsFilterData.ItemTypes = new ItemType[1]
                {
        ItemType.Schema
                };
                itemItemsFilterData.SchemaPurposes = purpose;*/

                // Use our filter to retrieve all the items the object our TCM refers to is being used by
                XElement allElements = this.Client.GetListXml("tcm:" + tcm, itemItemsFilterData);

                // Create a new string that will hold all the html we create to represent the information we retrieved above
                StringBuilder sb = new StringBuilder("<h2>" + title + " contains:</h2>");

                // If we got no results here we display this information
                if (!allElements.HasElements)
                {
                    sb.Append(title + " does not contain any items");
                }
                else
                {
                    var usageList = new List<ItemUsageDetails>();

                    // If we got results we create a div to hold them
                    sb.Append("<div class=\"usedItems results\">");

                    // Then we grab all of our items as XElements
                    var usedItems = allElements.Elements();

                    // Use a function to create our "table" heading
                    sb.Append(CreateItemsHeading());

                    // For each item in our results we call a CreateItem function to create the appropriate html
                    foreach (XElement item in usedItems)
                    {
                        var id = item.Attribute("ID")?.Value;
                        int usageCount = this.WhereUsedCount(id);
                        usageList.Add(new ItemUsageDetails
                        {
                            Icon = item.Attribute("Icon")?.Value,
                            TcmId = id,
                            Title = item.Attribute("Title")?.Value,
                            WebDavLocation = item.Attribute("Path")?.Value,
                            UsedCount = usageCount
                        });
                    }

                    // Do sorting
                    var sortedList = usageList.OrderBy(x => x.UsedCount);

                    // Create HTML rows for each item
                    foreach (var itemUsageDetails in sortedList)
                    {
                        sb.Append(this.CreateItem(itemUsageDetails));
                    }

                    // Close the div we opened above
                    sb.Append("</div>");
                }

                // We're done with our core service client so we close it now to free resources
                this.Client.Close();

                // Return the html we've built.
                return sb.ToString();
            }
            catch (Exception ex)
            {
                // proper way of ensuring that the client gets closed... we close it in our try block above, then in a catch block if an exception is
                // thrown we abort it.
                if (this.Client != null)
                {
                    this.Client.Abort();
                }

                // we are rethrowing the original exception and just letting webapi handle it
                throw ex;
            }
        }

        /// <summary>
        /// Creates an HTML representation of a Tridion object, including its title, path and TCM ID
        /// </summary>
        /// <param name="item">An XElement containing all information on a Tridion item</param>
        /// <returns>Formatted HTML presentation of key information for Tridion items</returns>
        private string CreateItem(ItemUsageDetails item)
        {
            string html = "<div class=\"item\">";
            html += "<div class=\"icon\" style=\"background-image: url(/WebUI/Editors/CME/Themes/Carbon2/icon_v7.1.0.66.627_.png?name=" + item.Icon + "&size=16)\"></div>";
            html += "<div class=\"usagecount\">" + item.UsedCount + "</div>";
            html += "<div class=\"name\">" + item.Title + "</div>";
            html += "<div class=\"path\">" + item.WebDavLocation + "</div>";
            html += "<div class=\"id\">" + item.TcmId + "</div>";
            html += "</div>";
            return html;
        }

        /// <summary>
        /// Creates an HTML string containing the headings explaining our representation of a Tridion object's key information
        /// </summary>
        /// <returns>Formatted HTML presentation of the headings for key information for Tridion items</returns>
        private string CreateItemsHeading()
        {
            string html = "<div class=\"headings\">";
            html += "<div class=\"icon\">&nbsp</div>";
            html += "<div class=\"usagecount\">Used #</div>";
            html += "<div class=\"name\">Name</div>";
            html += "<div class=\"path\">Path</div>";
            html += "<div class=\"id\">ID</div></div>";

            return html;
        }

        /// <summary>
        /// Gets the where used count.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="tcmId">The TCM identifier.</param>
        /// <returns/>
        private int WhereUsedCount(string tcmId)
        {
            UsingItemsFilterData usingItemsFilterData1 = new UsingItemsFilterData();
            usingItemsFilterData1.BaseColumns = ListBaseColumns.Id;
            usingItemsFilterData1.IncludeLocalCopies = true;
            usingItemsFilterData1.IncludedVersions = VersionCondition.OnlyLatestVersions;
            UsingItemsFilterData usingItemsFilterData2 = usingItemsFilterData1;
            return this.Client.GetList(tcmId, usingItemsFilterData2).Length;
        }

        /*

        /// <summary>
        /// Recursive function which checks a Tridion object's using items for pages, then moves on to any using items using items to check for pages, etc.
        /// </summary>
        /// <param name="client">Tridion core service client to be used to look up the page using our object</param>
        /// <param name="count">The number of jumps we've used made from one item to another item using it</param>
        /// <param name="depth">The number of above jumps we should allow</param>
        /// <param name="html">The HTML string we take in and add our new html to</param>
        /// <param name="tcm">The TCM ID of the Tridion object we are checking for pages on which it is used, or of an item using this object</param>
        /// <returns>Formatted HTML containing the list of pages using our Tridion object, which is added to with each recursive call.</returns>
        private string GetPages(string tcm, SessionAwareCoreServiceClient client, string html, int depth, int count)
        {
            // Create a UsingItemsFilter to get the items using the input Tridion object
            UsingItemsFilterData filter = new UsingItemsFilterData();

            // Sets the included versions for the filter to only retrieve the latest versions
            filter.IncludedVersions = VersionCondition.OnlyLatestVersions;

            // Get a list of all items using this object
            XElement usingItemsXElement = client.GetListXml(tcm, filter);
            var usingItems = usingItemsXElement.Elements();

            // For each of these items we check if they are a page
            foreach (XElement item in usingItems)
            {
                if (item.Attribute("Type").Value.Equals("64") && !html.Contains(item.Attribute("ID").Value))
                {
                    // If so we create a new item in our HTML
                    html += CreateItem(item);
                }
                else
                {
                    // Otherwise we check if our count matches our depth, in which case we stop, otherwise we recursively call this function for the
                    // new item, incrementing our count by one
                    if (count != depth)
                    {
                        html = GetPages(item.Attribute("ID").Value, client, html, depth, count + 1);
                    }
                }
            }

            // Return the modified html string
            return html;
        }*/
    }
}