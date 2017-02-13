namespace Sitecore.Js.Presentation.Managers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web.Hosting;

    using JavaScriptEngineSwitcher.ChakraCore;
    using JavaScriptEngineSwitcher.Core;

    using JSPool;

    using Sitecore.Js.Presentation.Configuration;

    public class JsEngineManager : IJsEngineManager
    {
        private const string EnsureInitCommandIsNotEmpty = "ENSURE_NOT_EMPTY";

        private const string ScriptsAreLoaded = "SCRIPTS_ARE_LOADED";

        private readonly IJsEngineManagerConfiguration _config;

        private readonly IList<string> _modules;

        private readonly JsPool _pool;

        private readonly JsPoolConfig _poolConfig;

        public JsEngineManager(IJsEngineManagerConfiguration config)
        {
            this._config = config;
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this._modules = config.Modules ?? new List<string>();

            // replace with custom object & mapping
            this._poolConfig = new JsPoolConfig
                                   {
                                       EngineFactory = new ChakraCoreJsEngineFactory().CreateEngine,
                                       Initializer = this.Initializer()
                                   };

            if (config.StartEngines.HasValue)
            {
                this._poolConfig.StartEngines = config.StartEngines.Value;
            }

            if (config.MaxEngines.HasValue)
            {
                this._poolConfig.MaxEngines = config.MaxEngines.Value;
            }

            if (config.MaxUsagesPerEngine.HasValue)
            {
                this._poolConfig.MaxUsagesPerEngine = config.MaxUsagesPerEngine.Value;
            }

            this._pool = new JsPool(this._poolConfig);
        }

        public virtual string CombineInitializationScripts()
        {
            var sb = new StringBuilder();

            foreach (var module in this._modules)
            {
                var filePath = module.Replace('/', '\\').TrimStart('~').TrimStart('\\');
                filePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath ?? string.Empty, filePath);

                if (File.Exists(filePath))
                {
                    sb.AppendLine(File.ReadAllText(filePath));
                }
            }

            return sb.ToString();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual IEngine GetEngine()
        {
            Func<IJsEngine> getEngine = () => this._pool.GetEngine(this._poolConfig.GetEngineTimeout);

            return new Engine(getEngine, this._pool.ReturnEngineToPool, this.AreScriptsLoaded);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._pool?.Dispose();
            }
        }

        private bool AreScriptsLoaded(IJsEngine engine)
        {
            return engine.HasVariable(ScriptsAreLoaded);
        }

        private Action<IJsEngine> Initializer()
        {
            var script = this.CombineInitializationScripts();

            Action<IJsEngine> action = (engine) =>
                {
                    var thisAssembly = typeof(JsEngineManager).Assembly;

                    engine.ExecuteResource("Sitecore.Js.Presentation.Resources.init.js", thisAssembly);

                    if (!string.IsNullOrEmpty(script))
                    {
                        engine.Execute(script);
                    }

                    engine.SetVariableValue(ScriptsAreLoaded, "true");
                };

            return action;
        }
    }
}