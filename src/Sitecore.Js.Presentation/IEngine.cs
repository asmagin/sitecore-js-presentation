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

    /// <summary>
    ///     Request-specific ReactJS.NET environment. This is unique to the individual request and is
    ///     not shared.
    /// </summary>
    public interface IEngine : IDisposable
    {
        /// <summary>
        ///     Executes the provided JavaScript code.
        /// </summary>
        /// <param name="code">JavaScript to execute</param>
        void Execute(string code);

        /// <summary>
        ///     Executes the provided JavaScript code, returning a result of the specified type.
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="code">Code to execute</param>
        /// <returns>Result of the JavaScript code</returns>
        T Execute<T>(string code);

        /// <summary>
        ///     Executes the provided JavaScript function, returning a result of the specified type.
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="function">JavaScript function to execute</param>
        /// <param name="args">Arguments to pass to function</param>
        /// <returns>Result of the JavaScript code</returns>
        T Execute<T>(string function, params object[] args);
    }
}