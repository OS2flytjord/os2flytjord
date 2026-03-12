using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Niras.Jordflytning.App_Start
{

    // Håndterer partial views med underscore fra PartialViewResult metoder
    public class RazorUnderscoreViewEngine : RazorViewEngine
    {
        public RazorUnderscoreViewEngine()
        {
            var underScored = new[] {"~/Views/{1}/_{0}.cshtml", "~/Views/{1}/_{0}.vbhtml"};
            PartialViewLocationFormats = underScored.Union(PartialViewLocationFormats).ToArray();
        }
    }

    public class ViewEngineConfig
    {
        public static void Configure()
        {
            ViewEngines.Engines.Remove(ViewEngines.Engines.Single(x => x is RazorViewEngine));
            ViewEngines.Engines.Add(new RazorUnderscoreViewEngine());
        }

    }
}