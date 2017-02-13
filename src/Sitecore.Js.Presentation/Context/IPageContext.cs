namespace Sitecore.Js.Presentation.Context
{
    using System.Collections.Generic;

    public interface IPageContext
    {
        List<string> InitScripts { get; }

        void Add(string client);
    }
}