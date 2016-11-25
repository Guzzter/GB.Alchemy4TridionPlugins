namespace ExportUserList.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web.Http;
    using System.Xml.Linq;

    using Alchemy4Tridion.Plugins;

    using Tridion.ContentManager.CoreService.Client;

    /// <summary>
    /// An ApiController to create web services that your plugin can interact with.
    /// </summary>
    /// <remarks>
    /// The AlchemyRoutePrefix accepts a Service Name as its first parameter. This will be used by both the generated Url's as well as the generated JS
    /// proxy. <c>/Alchemy/Plugins/{YourPluginName}/api/{ServiceName}/{action}</c><c>Alchemy.Plugins.YourPluginName.Api.Service.action()</c>
    ///
    /// The attribute is optional and if you exclude it, url's and methods will be attached to "api" instead. <c>/Alchemy/Plugins/{YourPluginName}/api/{action}</c><c>Alchemy.Plugins.YourPluginName.Api.action()</c>
    /// </remarks>
    [AlchemyRoutePrefix("ExportService")]
    public class PluginController : AlchemyApiController
    {
        /// // GET /Alchemy/Plugins/{YourPluginName}/api/{YourServiceName}/YourRoute
        [HttpGet]
        [Route("UserListToCsv")]
        public HttpResponseMessage UserListToCsv()
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(this.BuildUserListAsCsv());
                writer.Flush();
                stream.Position = 0;

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = string.Format("UserListExport-{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss"))
                };
                return result;
            }
            catch (Exception ex)
            {
#if RELEASE

                //throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
#endif
#if DEBUG

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.Source);
                sb.AppendLine(ex.StackTrace);
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StringContent(sb.ToString());
                return result;
#endif
            }
        }

        private void AddCsvLine(StringBuilder sb, params string[] items)
        {
            //Filter out the seperator char from the value to prevent wrong columns
            items = items.Select(x => x.Replace(",", "")).ToArray();

            //sb.AppendFormat("\u0022{0}\u0022", string.Join("\u0022,\u0022", items)).AppendLine();
            sb.AppendLine(string.Join(",", items));
        }

        /// <summary>
        /// Builds the user list as CSV.
        /// </summary>
        /// <returns></returns>
        private string BuildUserListAsCsv()
        {
            StringBuilder sb = new StringBuilder();
            this.AddCsvLine(sb, "Nr.", "Title", "Id", "Description", "Is enabled", "Is administrator");
            UsersFilterData usersFilterData = new UsersFilterData
            {
                BaseColumns = ListBaseColumns.Id,
                IsPredefined = false,
                ItemType = ItemType.User
            };

            int rowCounter = 1;

            foreach (XElement itemXml in this.Client.GetSystemWideListXml(usersFilterData).Elements())
            {
                UserData user = (UserData)this.Client.Read(itemXml.Attribute("ID").Value, new ReadOptions());
                this.AddCsvLine(
                    sb,
                    "" + rowCounter++,
                    user.Title,
                    user.Id,
                    user.Description,
                    (user.IsEnabled.HasValue ? user.IsEnabled.Value : false).ToString(),
                    ((user.Privileges.HasValue ? user.Privileges.Value : 0) == 1).ToString());
            }

            return sb.ToString();
        }
    }
}