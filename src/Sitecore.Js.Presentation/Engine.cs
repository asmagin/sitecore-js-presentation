/*
 *  Copyright (c) 2014-Present, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

namespace Sitecore.Js.Presentation
{
    using System;

    using JavaScriptEngineSwitcher.Core;

    /// <summary>
    ///     Request-specific ReactJS.NET environment. This is unique to the individual request and is
    ///     not shared.
    /// </summary>
    public class Engine : IEngine
    {
        /// <summary>
        ///     Contains an engine acquired from a pool of engines.
        /// </summary>
        protected Lazy<IJsEngine> EngineFromPool;

        private readonly Func<IJsEngine, bool> _areScriptsLoaded;

        private bool _disposed = false;

        public Engine(
            Func<IJsEngine> getEngineAction,
            Func<IJsEngine, bool> areScriptsLoaded)
        {
            this._areScriptsLoaded = areScriptsLoaded;

            this.EngineFromPool = new Lazy<IJsEngine>(getEngineAction);
        }

        protected IJsEngine JsEngine => this.EngineFromPool.Value;

        public virtual bool CheckScriptsReadiness()
        {
            if (!this._areScriptsLoaded(this.JsEngine))
            {
                throw new InvalidOperationException("Scripts are not ready for further execution");
            }

            return true;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// 
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                EngineFromPool?.Value?.Dispose();
                this.EngineFromPool = null;
            }

            // Free any unmanaged objects here.
            _disposed = true;
        }

        /// <summary>
        ///     Executes the provided JavaScript code.
        /// </summary>
        /// <param name="code">JavaScript to execute</param>
        public virtual void Execute(string code)
        {
            try
            {
                this.JsEngine.Execute(code);
            }
            catch (JsRuntimeException ex)
            {
                throw this.WrapJavaScriptRuntimeException(ex);
            }
        }

        /// <summary>
        ///     Executes the provided JavaScript code, returning a result of the specified type.
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="code">Code to execute</param>
        /// <returns>Result of the JavaScript code</returns>
        public virtual T Execute<T>(string code)
        {
            try
            {
                return this.JsEngine.Evaluate<T>(code);
            }
            catch (JsRuntimeException ex)
            {
                throw this.WrapJavaScriptRuntimeException(ex);
            }
        }

        /// <summary>
        ///     Executes the provided JavaScript function, returning a result of the specified type.
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="function">JavaScript function to execute</param>
        /// <param name="args">Arguments to pass to function</param>
        /// <returns>Result of the JavaScript code</returns>
        public virtual T Execute<T>(string function, params object[] args)
        {
            try
            {
                return this.JsEngine.CallFunction<T>(function, args);
            }
            catch (JsRuntimeException ex)
            {
                throw this.WrapJavaScriptRuntimeException(ex);
            }
        }

        /// <summary>
        ///     Determines if the specified variable exists in the JavaScript engine
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <returns><c>true</c> if the variable exists; <c>false</c> otherwise</returns>
        public virtual bool HasVariable(string name)
        {
            try
            {
                return this.JsEngine.HasVariable(name);
            }
            catch (JsRuntimeException ex)
            {
                throw this.WrapJavaScriptRuntimeException(ex);
            }
        }

        /// <summary>
        ///     Updates the Message of a <see cref="JsRuntimeException" /> to be more useful, containing
        ///     the line and column numbers.
        /// </summary>
        /// <param name="ex">Original exception</param>
        /// <returns>New exception</returns>
        protected virtual JsRuntimeException WrapJavaScriptRuntimeException(JsRuntimeException ex)
        {
            return
                new JsRuntimeException(
                    string.Format("{0}\r\nLine: {1}\r\nColumn:{2}", ex.Message, ex.LineNumber, ex.ColumnNumber),
                    ex.EngineName,
                    ex.EngineVersion)
                {
                    ErrorCode = ex.ErrorCode,
                    Category = ex.Category,
                    LineNumber = ex.LineNumber,
                    ColumnNumber = ex.ColumnNumber,
                    SourceFragment = ex.SourceFragment,
                    Source = ex.Source
                };
        }
    }
}