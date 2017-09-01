using System.Diagnostics;
using System.Linq;

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

    using Configuration;

    public class JsEngineManager : IJsEngineManager
    {
        private const string ScriptsAreLoaded = "SCRIPTS_ARE_LOADED";

        private readonly IList<string> _modules;

        private readonly JsPool _pool;

        private readonly JsPoolConfig _poolConfig;

        private string _scripts;

        public JsEngineManager(IJsEngineManagerConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this._modules = ResolvePaths(config.Modules ?? new List<string>()).ToList();

            // replace with custom object & mapping
            this._poolConfig = new JsPoolConfig
            {
                EngineFactory = new ChakraCoreJsEngineFactory().CreateEngine,
                Initializer = this.Initializer()
            };

            if (this._modules.Count > 0)
            {
                // Get common root path for all files and configure watching.
                this._poolConfig.WatchPath = this.GetCommonRoot(this._modules);
                this._poolConfig.WatchFiles = this._modules;
            }

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

            // Clean combined scripts on any script file change
            this._pool.Recycled += (sender, args) => { this._scripts = null; };
        }

        public virtual string CombineInitializationScripts()
        {
            var sb = new StringBuilder();

            foreach (var filePath in this._modules)
            {
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
            return new Engine(getEngine, this.AreScriptsLoaded);
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

        private string GetCombinedInitializationScripts
        {
            get
            {
                if (string.IsNullOrEmpty(this._scripts))
                {
                    this._scripts = this.CombineInitializationScripts();
                }

                return this._scripts;
            }
        }

        private Action<IJsEngine> Initializer()
        {
            Action<IJsEngine> action = (engine) =>
                {
                    var thisAssembly = typeof(JsEngineManager).Assembly;

                    engine.ExecuteResource("Sitecore.Js.Presentation.Resources.init.js", thisAssembly);

                    if (!string.IsNullOrEmpty(this.GetCombinedInitializationScripts))
                    {
                        engine.Execute(this.GetCombinedInitializationScripts);
                    }

                    engine.SetVariableValue(ScriptsAreLoaded, "true");
                };

            return action;
        }
        private IList<string> ResolvePaths(IList<string> values)
        {
            if (values != null && values.Any())
            {
                return values
                    .Select(x => x.Replace('/', '\\').TrimStart('~').TrimStart('\\'))
                    .Select(x => Path.GetFullPath(Path.Combine(HostingEnvironment.ApplicationPhysicalPath ?? string.Empty, x)))
                    .ToList();
            }

            return new List<string>();
        }

        private string GetCommonRoot(IList<string> values)
        {
            if (values == null || !values.Any())
            {
                return null;
            }

            var s = values
                .Where(File.Exists)
                .Select(file => Directory.GetParent(file).FullName)
                .Select(file => file.Split(new []{"\\"}, StringSplitOptions.RemoveEmptyEntries))
                .ToArray();

            var k = s[0].Length;
            for (var i = 1; i < s.Length; i++)
            {
                k = Math.Min(k, s[i].Length);
                for (var j = 0; j < k; j++)
                    if (s[i][j] != s[0][j])
                    {
                        k = j;
                        break;
                    }
            }

            var potentialRoot = string.Join("\\", s[0].Take(k));

            return Directory.Exists(potentialRoot)
                ? potentialRoot
                : null;
        }

    }
}