namespace Sitecore.Js.Presentation.Managers
{
    using System;

    public interface IJsEngineManager : IDisposable
    {
        string CombineInitializationScripts();

        IEngine GetEngine();
    }
}