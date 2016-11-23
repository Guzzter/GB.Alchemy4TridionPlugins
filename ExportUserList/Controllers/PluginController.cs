using System;
using System.Net.Http;
using System.Web.Http;
using Alchemy4Tridion.Plugins;

namespace ExportUserList.Controllers
{
    using System.IO;
    using System.Net;
    using System.Net.Http.Headers;

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
            writer.Write("Hello, World!");
            writer.Flush();
            stream.Position = 0;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("text/csv");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = "Export.csv" };
            return result;
        }
    }
}