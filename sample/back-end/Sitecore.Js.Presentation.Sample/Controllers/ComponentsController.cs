namespace Sitecore.Js.Presentation.Sample.Controllers
{
    using System.Web.Mvc;

    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Js.Presentation.Enum;
    using Sitecore.Js.Presentation.Mvc;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Web.UI.WebControls;

    using Context = Sitecore.Context;

    public class ComponentsController : JsController
    {
        public ActionResult Counter()
        {
            var dataSource = this.DataSource();

            var model = new { title = FieldRenderer.Render(dataSource, "Title"), };

            return this.JsComponent("common__ext.counter", model, RenderingOptions.ClientAndServer);
        }

        public Item DataSource()
        {
            var datasourceId = RenderingContext.Current.Rendering.DataSource;

            if (ID.IsID(datasourceId))
            {
                return Context.Database.GetItem(new ID(datasourceId));
            }

            return Context.Item;
        }

        public ActionResult LodashContent()
        {
            var dataSource = this.DataSource();

            var model =
                new
                    {
                        title = FieldRenderer.Render(dataSource, "Title"),
                        text = FieldRenderer.Render(dataSource, "Text"),
                        image = FieldRenderer.Render(dataSource, "Image")
                    };

            return this.JsComponent("common__ext.lodashContent", model, RenderingOptions.ServerOnly);
        }

        public ActionResult RichText()
        {
            return this.View(this.DataSource());
        }

        public ActionResult SimpleContent()
        {
            var dataSource = this.DataSource();

            var model =
                new
                    {
                        title = FieldRenderer.Render(dataSource, "Title"),
                        text = FieldRenderer.Render(dataSource, "Text"),
                        image = FieldRenderer.Render(dataSource, "Image")
                    };

            return this.JsComponent("common__ext.simpleContent", model, RenderingOptions.ServerOnly);
        }

        public ActionResult Timer()
        {
            var dataSource = this.DataSource();

            var model = new { title = FieldRenderer.Render(dataSource, "Title"), };

            return this.JsComponent("common__ext.timer", model, RenderingOptions.ClientAndServer);
        }
    }
}