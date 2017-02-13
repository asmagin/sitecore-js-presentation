namespace Sitecore.Js.Presentation.Pipelines
{
    using Sitecore.Js.Presentation.Configuration;
    using Sitecore.Pipelines;

    public class InitializeJsContext
    {
        public IJsEngineManagerConfiguration Config { get; set; }

        public void Process(PipelineArgs args)
        {
            Locator.Initialize(this.Config);
        }
    }
}