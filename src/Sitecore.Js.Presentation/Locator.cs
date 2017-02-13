namespace Sitecore.Js.Presentation
{
    using System;

    using Sitecore.Js.Presentation.Configuration;
    using Sitecore.Js.Presentation.Context;
    using Sitecore.Js.Presentation.Managers;

    public class Locator
    {
        private static readonly object SyncObject = new object();

        private static Locator _current;

        protected Locator(IJsEngineManagerConfiguration config)
        {
            this.Page = new PageContext();
            this.Manager = new JsEngineManager(config);
        }

        public static Locator Current
        {
            get
            {
                if (_current == null)
                {
                    throw new Exception("JsEngine is not initialized yet");
                }

                return _current;
            }
        }

        // ReSharper disable once ConvertToAutoProperty
        public IJsEngineManager Manager { get; }

        public IPageContext Page { get; }

        public static void Initialize(IJsEngineManagerConfiguration config)
        {
            if (_current != null)
            {
                throw new Exception("JsEngine is already initialized");
            }

            lock (SyncObject)
            {
                if (_current == null)
                {
                    _current = new Locator(config);
                }
            }
        }
    }
}