namespace Sitecore.Js.Presentation.Configuration
{
    using System.Collections.Generic;

    public interface IJsEngineManagerConfiguration
    {
        /// <summary>
        ///     Gets or sets the maximum number of engines that will be created in the pool.
        ///     Defaults to <c>25</c>.
        /// </summary>
        int? MaxEngines { get; set; }

        int? MaxUsagesPerEngine { get; set; }

        IList<string> Modules { get; set; }

        /// <summary>
        ///     Gets or sets the number of engines to initially start when a pool is created.
        ///     Defaults to <c>10</c>.
        /// </summary>
        int? StartEngines { get; set; }

        void AddModule(string module);
    }
}