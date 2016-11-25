namespace SaveCloseAndPublish.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Xml.Linq;

    using Alchemy4Tridion.Plugins;

    using SaveCloseAndPublish.GUI;

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
    [AlchemyRoutePrefix("PublishService")]
    public class PluginController : AlchemyApiController
    {
        /// Get /Alchemy/Plugins/{YourPluginName}/api/{YourServiceName}/YourRoute
        [HttpGet]
        [Route("SaveAndPublish/{tcm}")]
        public string SaveAndPublish(string tcm)
        {
            string response = "Page '{0}' is published to: ";
            try
            {
                tcm = "tcm:" + tcm;
                PublishSettings settings = this.Plugin.Settings.Get<PublishSettings>();
                var pageData = this.Client.Read(tcm, new ReadOptions()) as PageData;
                if (pageData != null)
                {
                    if (pageData.IsEditable.HasValue && pageData.IsEditable.Value)
                    {
                        this.Client.Save(pageData, new ReadOptions());
                        this.Client.CheckIn(tcm, true, "Saved and published", new ReadOptions());
                    }
                    response = string.Format(response, pageData.Title);
                    response += this.PublishPage(tcm, pageData, settings.PublishPrio ?? string.Empty, settings.PublishTargetNamesCsv ?? string.Empty);
                }
                else
                {
                    throw new ArgumentException("Could not find page: " + tcm);
                }
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
                return sb.ToString();
#endif
            }
            return response;
        }

        public string PublishPage(string tcm, PageData page, string publishPriority, string publishTargets)
        {
            if (page == null)
            {
                throw new ArgumentException("Could not find page: " + tcm);
            }

            string pubId = page.LocationInfo.ContextRepository.IdRef;
            List<string> listPublishTargetNames = publishTargets.ToLowerInvariant().Split(',').Select(p => p.Trim()).ToList();
            List<KeyValuePair<string, string>> targets = this.TargetTypesForPublication(pubId);
            if (targets == null)
            {
                throw new ArgumentException("Skipped publishing, no suitable publish targets available for publication: " + pubId);
            }

            var items = targets.Where(x => listPublishTargetNames.Contains(x.Value)).ToList();
            string targetsPublished = string.Join(", ", items.Select(x => x.Value));

            if (items.Any())
            {
                List<string> listPublishTargetIds = new List<string>();

                foreach (var item in items)
                {
                    listPublishTargetIds.Add(item.Key);
                }

                PublishInstructionData instruction = new PublishInstructionData
                {
                    ResolveInstruction = new ResolveInstructionData { IncludeChildPublications = false },
                    RenderInstruction = new RenderInstructionData()
                };

                this.Client.Publish(
                    new[] { page.Id },
                    instruction,
                    listPublishTargetIds.ToArray(),
                    this.GetPublishPriority(publishPriority),
                    null);
            }
            else
            {
                throw new Exception("Skipped publishing, no suitable publish targets configured.");
            }

            return targetsPublished;
        }

        /// <summary>
        /// Gets the publish priority.
        /// </summary>
        /// <param name="publishPriority">The publish priority.</param>
        /// <param name="defaultReturnValue">The default return value.</param>
        /// <returns>The PublishPriority enum</returns>
        public PublishPriority GetPublishPriority(string publishPriority, PublishPriority defaultReturnValue = PublishPriority.Low)
        {
            PublishPriority priority = defaultReturnValue;

            try
            {
                if (!Enum.TryParse(publishPriority, true, out priority))
                {
                    priority = defaultReturnValue;
                }
            }
            catch
            {
                ////default to low priority
            }

            return priority;
        }

        /// <summary>
        /// Get all target types from publication
        /// </summary>
        /// <param name="publicationId">Publication id</param>
        /// <returns>XElement with list of target types</returns>
        public List<KeyValuePair<string, string>> TargetTypesForPublication(string publicationId)
        {
            try
            {
                var targetTypesFilter = new TargetTypesFilterData();
                targetTypesFilter.ForRepository = new LinkToRepositoryData()
                {
                    IdRef = publicationId
                };
                XElement allTargetTypes = this.Client.GetSystemWideListXml(targetTypesFilter);
                return allTargetTypes.Descendants().Select(item => new KeyValuePair<string, string>(item.Attribute("ID").Value, item.Attribute("Title").Value.ToLowerInvariant())).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}