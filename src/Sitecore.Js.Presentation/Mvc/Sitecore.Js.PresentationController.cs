namespace Sitecore.Js.Presentation.Mvc
{
    using System.IO;
    using System.Web.Mvc;

    using Sitecore.Diagnostics;
    using Sitecore.Js.Presentation.Enum;
    using Sitecore.Mvc.Controllers;
    using Sitecore.Mvc.Pipelines;
    using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
    using Sitecore.Mvc.Presentation;

    public class JsController : SitecoreController
    {
        /// <summary>
        ///     Generate component ReactJS action results
        /// </summary>
        /// <param name="componentName">Name of a component exposed from a js package</param>
        /// <param name="model">Data to be passed into a component</param>
        /// <param name="renderOptions"></param>
        /// <returns>ActionResult object</returns>
        public virtual ActionResult JsComponent(string componentName, object model, RenderingOptions renderOptions)
        {
            return this.JsComponent(componentName, model, renderOptions, RenderingContext.Current.Rendering);
        }

        /// <summary>
        ///     Generate component ReactJS action results
        /// </summary>
        /// <param name="componentName">Name of a component exposed from a js package</param>
        /// <param name="model">Data to be passed into a component</param>
        /// <param name="rendering">Rendering definition object</param>
        /// <param name="renderOptions"></param>
        /// <returns>ActionResult object</returns>
        public virtual ActionResult JsComponent(
            string componentName,
            object model,
            RenderingOptions renderOptions,
            Rendering rendering)
        {
            var server = Locator.Current.Manager;
            var page = Locator.Current.Page;

            var component = new Component(server, page, componentName, model, rendering);

            // Render ReactJS component
            return this.Content(component.Render(renderOptions));
        }

        /// <summary>
        ///     Generate a content of a placeholder into  a string
        /// </summary>
        /// <param name="placeholderName">Name of a placeholder</param>
        /// <param name="rendering">Rendering definition object</param>
        /// <returns>Returns string with a rendered placeholder content</returns>
        protected virtual string Placeholder(string placeholderName, Rendering rendering)
        {
            Assert.ArgumentNotNull(placeholderName, "placeholderName");

            var stringWriter = new StringWriter();
            PipelineService.Get()
                .RunPipeline(
                    "mvc.renderPlaceholder",
                    new RenderPlaceholderArgs(placeholderName, stringWriter, rendering));
            return stringWriter.ToString();
        }
    }
}