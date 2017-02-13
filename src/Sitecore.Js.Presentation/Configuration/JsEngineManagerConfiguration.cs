namespace Sitecore.Js.Presentation.Configuration
{
    using System.Collections.Generic;

    using Sitecore.Diagnostics;

    public class JsEngineManagerConfiguration : IJsEngineManagerConfiguration
    {
        public JsEngineManagerConfiguration()
        {
            this.Modules = new List<string>();
        }

        public int? MaxEngines { get; set; }

        public int? MaxUsagesPerEngine { get; set; }

        public IList<string> Modules { get; set; }

        public int? StartEngines { get; set; }

        public void AddModule(string module)
        {
            Assert.ArgumentNotNull(module, "module");
            this.Modules.Add(module);
        }
    }
}