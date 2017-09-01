# Sitecore JavaScript Presentation Module

[![Build status](https://ci.appveyor.com/api/projects/status/w8k84te2f33xmmf9?svg=true)](https://ci.appveyor.com/project/asmagin/sitecore-js-presentation) [![NuGet version](http://img.shields.io/nuget/v/Sitecore.Js.Presentation.svg)](https://www.nuget.org/packages/Sitecore.Js.Presentation/)
[![Join the chat at https://gitter.im/sitecore-js-presentation/Lobby](https://badges.gitter.im/sitecore-js-presentation/Lobby.svg)](https://gitter.im/sitecore-js-presentation/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

A Sitecore module for building components using embedded JavaScript engine. A sample solution for module supports will give you examples of [ReactJS](https://facebook.github.io/react/) and [Lodash](https://lodash.com/) templates, but there is nothing that stopping you from creating components using different JS templating engine.

The module doesn't require custom renderings to be created in Sitecore. More about this approach you could find in my blog [https://asmagin.com](https://asmagin.com)

## Features
* Clean separation of Sitecore and Front-end Development.
* Server-side component rendering for JavaScript templating engines and [isomorphic](http://isomorphic.net/javascript) for ReactJs.
* Isomorphic renderings could support React + [Redux](http://redux.js.org/) 
* SEO support ( due to server-side enabled renderings )
* Full support for Sitecore Experience Editor

## Getting Started

- Install the NuGet package [Sitecore.Js.Presentation](https://www.nuget.org/packages/Sitecore.Js.Presentation/)
``` powershell
Install-Package Sitecore.Js.Presentation
```

- Create JavaScript bundle that will export components in following format **\<my_bundle\>.\<my_component\>** to global scope. Each component should implement followind functions that will be used from C# code to render data.
``` javascript
var my_bundle = {
    my_component: {
        renderToDOM: function(props, node) {
            // generate html and append it to DOM node
        },
        renderToString: function(props) {
            // generate html and return as string
        },
        renderToStaticMarkup: function(props) {
            // generate html and return as string
            
            // in this case e.g. React will not generate reactid into a DOM 
            // and won't be able to bing this code on a client side

            // could be used in only server-side rendering is needed
        },
        getPlaceholders: function(){
            // return array of placeholders names converted to JSON format
        }
    }
}
```

- Reference JS bundle in a configuration file. The NuGet package with create an empty config  
``` xml
<!-- File Path: .\App_Config\Include\Sitecore.Js.Presentation\Sitecore.Js.Presentation.config --> 

<Modules hint="list:AddModule">
    <Module>/path/to/my_bundle.js</Module>
    <Module>/path/to/my_bundle2.js</Module>
</Modules>
```

- Create Controller Rendering and reference is from JsController
``` csharp
namespace Sitecore.Js.Presentation.Sample.Controllers
{
    using System.Web.Mvc;

    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Js.Presentation.Enum;
    using Sitecore.Js.Presentation.Mvc;
    using Sitecore.Mvc.Presentation;
    using Sitecore.Web.UI.WebControls;

    using Context = Sitecore.Context;

    public class ComponentsController : JsController
    {
       ...
    }
}
```

- Add an Action that will generate data and pass it to a helper method
``` csharp
public ActionResult MyComponent()
{
    var dataSource = this.DataSource();

    var model =
        new
            {
                title = FieldRenderer.Render(dataSource, "Title"),
                text = FieldRenderer.Render(dataSource, "Text"),
                image = FieldRenderer.Render(dataSource, "Image")
            };

    return this.JsComponent("my_bundle.my_component", model, RenderingOptions.ServerOnly);
}
```

It is also possible to call Js rendering directly from ".cshtml" file.
``` html
<div>
    @Html.RenderComponent("my_bundle.other_component", new { })
</div>
```

## Sample
#### Solution

You could find sample solution in [this repo](./sample). It contains 2 projects:
- Front-end - node.js based sandbox for a front-end developer that uses [Webpack](https://webpack.github.io/) as a build tool and dev server. At the moment of creation of this module setup was pretty modern.
- Back-end - sample Visual Studio solution, that represents very basic case of using this module.

#### Sample Package
A Sample sitecore update package is aslo included. You could install it into a clean sitecore installation to see how this is working. The package includes few pages with different layouts and dynamic component based on React+Redux.

Package could be downloaded [here](https://github.com/asmagin/sitecore-js-presentation/raw/master/sample/sc-packages/Sitecore.Js.Presentation.Sample.Master.update)

To install the package use Sitecore Update Installation Wizard

Once the installation is done, you would need to update *web.config* and add assembly binding for JavaScriptEngineSwitcher:
``` xml
<runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
        <dependentAssembly>
            <assemblyIdentity name="JavaScriptEngineSwitcher.Core" publicKeyToken="c608b2a8cc9e4472" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-2.4.10.0" newVersion="2.4.10.0" />
        </dependentAssembly>
    </assemblyBinding>
</runtime>
```

## Feedback
Feel free to contact me here or on Twitter [@true_shoorik](https://twitter.com/true_shoorik) to discuss the module. Please, also [post issues](https://github.com/asmagin/sitecore-js-presentation/issues) if you find them. 
