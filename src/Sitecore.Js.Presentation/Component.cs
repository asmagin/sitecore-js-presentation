/*
 *  Copyright (c) 2014-Present, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

using System.ComponentModel;
using System.Dynamic;
using Sitecore.Extensions.StringExtensions;

namespace Sitecore.Js.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using JavaScriptEngineSwitcher.Core;

    using Newtonsoft.Json;

    using Sitecore.Diagnostics;
    using Sitecore.Js.Presentation.Context;
    using Sitecore.Js.Presentation.Enum;
    using Sitecore.Js.Presentation.Managers;
    using Sitecore.Mvc.Pipelines;
    using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
    using Sitecore.Mvc.Presentation;

    /// <summary>
    ///     Represents a React JavaScript component.
    /// </summary>
    public class Component : IComponent
    {
        private const string JsFunctionToGetPlaceholdersInfo = @"(function(obj){{
    if (typeof (obj) === 'undefined'
      || typeof (obj.getPlaceholders) === 'undefined'
      || typeof (obj.getPlaceholders) !== 'function' ) {{
      return '';
    }}

    return obj.getPlaceholders();
}})({0})";

        private readonly Regex _dynamicPlaceholderRegex = new Regex("(\\.|_|-|#)dynamic$", RegexOptions.IgnoreCase);
        
        /// <summary>
        ///     Regular expression used to validate JavaScript identifiers. Used to ensure component
        ///     names are valid.
        ///     Based off https://gist.github.com/Daniel15/3074365
        /// </summary>
        private static readonly Regex IdentifierRegex =
            new Regex(@"^[a-zA-Z_$][0-9a-zA-Z_$]*(?:\[(?:"".+""|\'.+\'|\d+)\])*?$", RegexOptions.Compiled);

        private JsonSerializerSettings _jsonSerializerSettings;

        private readonly IJsEngineManager _manager;

        private readonly IPageContext _pageContext;

        private readonly Rendering _rendering;

        public Component(
            IJsEngineManager manager,
            IPageContext pageContext,
            string componentName,
            object model,
            Rendering rendering,
            string site = null,
            string containerId = null,
            string containerTag = null,
            string containerClass = null)
        {
            this.ComponentName = EnsureComponentNameValid(componentName);

            this._manager = manager;
            this._pageContext = pageContext;
            this.Model = model;
            this._rendering = rendering;

            this.ContainerId = string.IsNullOrEmpty(containerId) ? GenerateId() : containerId;
            this.ContainerTag = string.IsNullOrEmpty(containerTag) ? "div" : containerTag;
            this.ContainerClass = containerClass;

            this._jsonSerializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        ///     Gets or sets the name of the component
        /// </summary>
        public string ComponentName { get; }

        /// <summary>
        ///     Gets or sets the HTML class for the container of this component
        /// </summary>
        public string ContainerClass { get; set; }

        /// <summary>
        ///     Gets or sets the unique ID for the DIV container of this component
        /// </summary>
        public string ContainerId { get; }

        /// <summary>
        ///     Gets or sets the HTML tag the component is wrapped in
        /// </summary>
        public string ContainerTag { get; set; }

        public object Model { get; set; }

        /// <summary>
        ///     Gets or sets the props for this component
        /// </summary>
        public virtual string Render(RenderingOptions options)
        {
            var placeholders = this.GetPlaceholders()
                .ToDictionary(name => _dynamicPlaceholderRegex.Replace(name, string.Empty), placeholder => this.Placeholder(placeholder, this._rendering));

            // Create ReactJS component props object
            dynamic props = ToDynamic(this.Model);
            props.placeholders = placeholders;

            var serializedProps = JsonConvert.SerializeObject(props, this._jsonSerializerSettings);

            var client = string.Empty;

            if (options != RenderingOptions.ServerOnly)
            {
                client = this.RenderJavaScript(serializedProps);
            }

            var server = this.RenderHtml(serializedProps, options);

            // Do not register in page context or return HTML rendered on server.
            this._pageContext.Add(client);

            return server;
        }

        /// <summary>
        ///     Validates that the specified component name is valid
        /// </summary>
        /// <param name="componentName"></param>
        internal static string EnsureComponentNameValid(string componentName)
        {
            var isValid = componentName.Split('.').All(segment => IdentifierRegex.IsMatch(segment));
            if (!isValid)
            {
                throw new Exception($"Invalid component name '{componentName}'");
            }

            return componentName;
        }

        /// <summary>
        ///     Ensures that this component exists in global scope
        /// </summary>
        protected virtual void EnsureComponentExists()
        {
            bool componentExists;

            // This is safe as componentName was validated via EnsureComponentNameValid()
            using (var engine = this._manager.GetEngine())
            {
                componentExists = engine.Execute<bool>($"typeof {this.ComponentName} !== 'undefined'");
            }

            if (!componentExists)
            {
                throw new Exception($"Could not find a component named '{this.ComponentName}'.");
            }
        }

        protected virtual IEnumerable<string> GetPlaceholders()
        {
            using (var engine = this._manager.GetEngine())
            {
                var jsOutpout =
                    engine.Execute<string>(string.Format(JsFunctionToGetPlaceholdersInfo, this.ComponentName))
                    ?? string.Empty;
                return JsonConvert.DeserializeObject<string[]>(jsOutpout) ?? new string[0];
            }
        }

        protected virtual string Placeholder(string placeholderName, Rendering rendering)
        {
            Assert.ArgumentNotNull(placeholderName, "placeholderName");
            Assert.ArgumentNotNull(rendering, "rendering");

            var stringWriter = new StringWriter();

            // Append placeholder name with "-dynamic", "_dynamic", ".dynamic" or "#dynamic" in JS component to treat this placeholder as Dynamic 
            if (_dynamicPlaceholderRegex.IsMatch(placeholderName))
            {
                // "-dynamic" part will be removed
                var placeholder = _dynamicPlaceholderRegex.Replace(placeholderName, string.Empty);

                // this will render placeholders with appended IDs, which is usually used for dynamic placeholders.
                // you do not need to specify dynamic placeholders in JS (as FE developer might not what is that)
                PipelineService.Get()
                    .RunPipeline(
                        "mvc.renderPlaceholder",
                        new RenderPlaceholderArgs($"{placeholder}_{rendering.UniqueId.ToString("D").ToUpper()}",
                            stringWriter, rendering));

                // TODO: figure out how to get correct index to append placeholder
                PipelineService.Get()
                    .RunPipeline(
                        "mvc.renderPlaceholder",
                        new RenderPlaceholderArgs($"{placeholder}-{rendering.UniqueId.ToString("B").ToUpper()}-0",
                            stringWriter, rendering));
            } else {
                // standard placeholder
                PipelineService.Get()
                    .RunPipeline(
                        "mvc.renderPlaceholder",
                        new RenderPlaceholderArgs(placeholderName, stringWriter, rendering));
            }

            return stringWriter.ToString();
        }

        /// <summary>
        ///     Renders the HTML for this component. This will execute the component server-side and
        ///     return the rendered HTML.
        /// </summary>
        /// <returns>HTML</returns>
        protected virtual string RenderHtml(string serializedProps, RenderingOptions options)
        {
            var renderServerOnly = options == RenderingOptions.ServerOnly;
            var renderNotClientOnly = options != RenderingOptions.ClientOnly;

            if (renderNotClientOnly)
            {
                this.EnsureComponentExists();
            }

            try
            {
                var html = string.Empty;
                if (renderNotClientOnly)
                {
                    var reactRenderCommand = renderServerOnly
                                                 ? $"{this.ComponentName}.renderToStaticMarkup({serializedProps})"
                                                 : $"{this.ComponentName}.renderToString({serializedProps})";

                    using (var engine = this._manager.GetEngine())
                    {
                        html = engine.Execute<string>(reactRenderCommand);
                    }
                }

                if (renderServerOnly)
                {
                    return html;
                }

                string attributes = $"id=\"{this.ContainerId}\"";

                if (!string.IsNullOrEmpty(this.ContainerClass))
                {
                    attributes += $" class=\"{this.ContainerClass}\"";
                }

                return string.Format("<{0} {1}>{2}</{0}>", this.ContainerTag, attributes, html);
            }
            catch (JsRuntimeException ex)
            {
                throw new Exception(
                    $"Error while rendering '{ComponentName}' to '{ContainerId}': {ex.Message}");
            }
        }

        /// <summary>
        ///     Renders the JavaScript required to initialise this component client-side. This will
        ///     initialise the React component, which includes attach event handlers to the
        ///     server-rendered HTML.
        /// </summary>
        /// <returns>JavaScript</returns>
        protected virtual string RenderJavaScript(string serializedProps)
        {
            return $"{this.ComponentName}.renderToDOM({serializedProps}, '{this.ContainerId}')";
        }

        /// <summary>
        ///     Generates a unique identifier for this component, if one was not passed in.
        /// </summary>
        /// <returns></returns>
        private static string GenerateId()
        {
            var str =
                Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                    .Replace("/", string.Empty)
                    .Replace("+", string.Empty)
                    .TrimEnd('=');

            return "react_" + str;
        }

        private ExpandoObject ToDynamic(object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
    }
}