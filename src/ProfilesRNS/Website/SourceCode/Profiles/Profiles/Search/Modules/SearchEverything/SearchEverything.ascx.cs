using System;
using System.Collections.Generic;
using System.Xml;

using Profiles.Framework.Utilities;

namespace Profiles.Search.Modules.SearchEverything
{
    public partial class SearchEverything : BaseModule
    {
        public SearchEverything() { }
        public SearchEverything(XmlDocument pagedata, List<ModuleParams> moduleparams, XmlNamespaceManager pagenamespaces)
            : base(pagedata, moduleparams, pagenamespaces)
        {
        }
    }
}