using System;
using System.Net.Http;
using System.Web.Http;
using Alchemy4Tridion.Plugins;

namespace ExportUserList.Controllers
{
    using System.IO;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Xml.Linq;

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
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(BuildUserListAsCsv());
            writer.Flush();
            stream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
            return result;
        }

        private void AddCsvLine(StringBuilder sb, params string[] items)
        {
            sb.AppendFormat("\u0022{0}\u0022", string.Join("\u0022,\u0022", items)).AppendLine();
        }

        /// <summary>
        /// Builds the user list as CSV.
        /// </summary>
        /// <returns></returns>
        private string BuildUserListAsCsv()
        {
            SessionAwareCoreServiceClient client = null;

            try
            {
                // Creates a new core service client
                client = new SessionAwareCoreServiceClient("netTcp_2013");

                UsersFilterData usersFilterData = new UsersFilterData
                {
                    BaseColumns = ListBaseColumns.Id,
                    IsPredefined = false,
                    ItemType = ItemType.User
                };

                int i = 1;
                StringBuilder sb = new StringBuilder();
                AddCsvLine(sb, "Nr.", "Title", "Id", "Description", "Is enabled", "Is administrator");
                foreach (XElement itemXml in client.GetSystemWideListXml(usersFilterData).Elements())
                {
                    UserData user = (UserData)client.Read(itemXml.Attribute("ID").Value, new ReadOptions());
                    AddCsvLine(
                        sb,
                        "" + i,
                        user.Title,
                        user.Id,
                        user.Description,
                        user.IsEnabled.ToString(),
                        user.Privileges.ToString());
                }
                return sb.ToString();
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
    }
}