namespace Sitecore.Js.Presentation.Context
{
    using System.Collections.Generic;
    using System.Web;

    public class PageContext : IPageContext
    {
        private const string ComponentsStorageNameInHttpContext = "300C5ADADC894A128C4A59A9BB530117";

        public List<string> InitScripts
        {
            get
            {
                var obj = HttpContext.Current.Items[ComponentsStorageNameInHttpContext];

                if (obj != null)
                {
                    return (List<string>)obj;
                }

                var list = new List<string>();
                HttpContext.Current.Items[ComponentsStorageNameInHttpContext] = list;
                return list;
            }
        }

        public void Add(string script)
        {
            this.InitScripts.Insert(0, script);
        }
    }
}