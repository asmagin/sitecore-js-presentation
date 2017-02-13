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
    using Sitecore.Js.Presentation.Enum;

    /// <summary>
    ///     Represents a React JavaScript component.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        ///     Gets or sets the name of the component
        /// </summary>
        string ComponentName { get; }

        /// <summary>
        ///     Gets or sets the HTML class for the container of this component
        /// </summary>
        string ContainerClass { get; set; }

        /// <summary>
        ///     Gets or sets the unique ID for the container of this component
        /// </summary>
        string ContainerId { get; }

        /// <summary>
        ///     Gets or sets the HTML tag the component is wrapped in
        /// </summary>
        string ContainerTag { get; set; }

        /// <summary>
        ///     Gets or sets the props for this component
        /// </summary>
        object Model { get; set; }

        /// <summary>
        ///     Renders the HTML for this component. This will execute the component server-side and
        ///     return the rendered HTML.
        /// </summary>
        /// <returns>HTML</returns>
        string Render(RenderingOptions options);
    }
}