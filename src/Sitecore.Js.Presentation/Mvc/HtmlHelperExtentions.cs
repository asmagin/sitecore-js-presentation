namespace Sitecore.Js.Presentation.Mvc
{
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using Sitecore.Js.Presentation.Enum;
    using Sitecore.Mvc.Presentation;

    public static class HtmlHelperExtentions
    {
        public static IHtmlString RenderComponent(this HtmlHelper htmlHelper, string componentName, object model)
        {
            // Render ReactJS component
            return RenderComponent(htmlHelper, componentName, model, RenderingOptions.ClientAndServer);
        }

        public static IHtmlString RenderComponent(
            this HtmlHelper htmlHelper,
            string componentName,
            object model,
            RenderingOptions renderOptions)
        {
            var server = Locator.Current.Manager;
            var page = Locator.Current.Page;
            var rendering = RenderingContext.Current.Rendering;

            var component = new Component(server, page, componentName, model, rendering);

            // Render ReactJS component
            return new HtmlString(component.Render(renderOptions));
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            var sb = new StringBuilder();

            foreach (var str in Locator.Current.Page.InitScripts)
            {
                sb.AppendLine(str);
            }

            var tag = new TagBuilder("script") { InnerHtml = sb.ToString() };
            return new HtmlString(tag.ToString());
        }
    }
}