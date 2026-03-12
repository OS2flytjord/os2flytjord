using System.Configuration;
using System.Web.Optimization;

namespace Niras.Jordflytning.App_Start
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
		{
			//OBS: Virtuel path i scriptBundle constructoren må ikke være en virklig placering på websitet - så virker det ikke.
			//http://support.appharbor.com/discussions/problems/4700-403-access-denied-when-using-scriptbundle

			// Javascript

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jQuery/jquery-{version}.min.js", "~/Scripts/jQuery/jquery.iecors.js"));
            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jQuery/jquery-ui-{version}.min.js", "~/Scripts/jquery/jquery.ui.datepicker-da.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.min.js", "~/Scripts/jquery/jquery.ui.datepicker-da.js"));
			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jQuery/jquery.unobtrusive-ajax.js", "~/Scripts/jQuery/jquery.validate*", "~/Scripts/mvcfoolproof.unobtrusive.min.js"));
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
			bundles.Add(new ScriptBundle("~/bundles/eventlib").Include("~/Scripts/eventlib.js"));
            bundles.Add(new ScriptBundle("~/bundles/kortviser", "https://js.kortinfo.net/public/1.0/Niras.Public.js"));
			//bundles.Add(new ScriptBundle("~/bundles/jquerymobile").Include("~/Scripts/jQueryMobile/jquery.mobile-{version}.js"));
            			bundles.Add(new ScriptBundle("~/bundles/jquerymobile").Include("~/Scripts/jQueryMobile/jquery.mobile-{version}.js"));
			bundles.Add(new ScriptBundle("~/bundles/jqmAutoComplete").Include("~/Scripts/jqm.autoComplete-1.5.2-min.js"));
			bundles.Add(new ScriptBundle("~/bundles/proj4s").Include("~/Scripts/Proj4s/proj4js.combined.js", "~/Scripts/Proj4s/defs/EPSG25832.js"));
            bundles.Add(new ScriptBundle("~/bundles/wicket").Include("~/Scripts/Wicket/wicket.js"));
			bundles.Add(new ScriptBundle("~/bundles/bootstrapDatepicker").Include("~/Scripts/bootstrap-datepicker.js"));

      // The Kendo JavaScript bundle
      bundles.Add(new ScriptBundle("~/bundles/kendo").Include("~/Scripts/Kendo/kendo.web.*",// or kendo.all.* if you want to use Kendo UI Web and Kendo UI DataViz
                                                              "~/Scripts/Kendo/kendo.aspnetmvc.*",
                                                              "~/Scripts/Kendo/kendo.culture.*",
                                                              "~/Scripts/Kendo/kendo.da-DK.*", 
															  "~/Scripts/Kendo/kendoExcelGrid.*"
                                                              ));

			// Iframe auto-height JavaScript bundle: https://github.com/house9/jquery-iframe-auto-height
			bundles.Add(new ScriptBundle("~/bundles/iframeautoheight").Include("~/Scripts/jQuery/jquery.browser.js", "~/Scripts/jQuery/jquery.iframe-auto-height.js"));

			// resize JavaScript bundle: http://benalman.com/projects/jquery-resize-plugin/
			bundles.Add(new ScriptBundle("~/bundles/resize").Include("~/Scripts/jQuery/jquery.resize.js"));

			// Jordflytning specific JavaScripts
			bundles.Add(new ScriptBundle("~/bundles/jordflytning").Include("~/Scripts/Jordflytning.js"));
			bundles.Add(new ScriptBundle("~/bundles/mobile").Include("~/Scripts/Mobile/minside.js", "~/Scripts/Mobile/loginpage.js").Include("~/Scripts/Mobile/frontpage.js"));

            // Cookie disclaimer
            bundles.Add(new ScriptBundle("~/bundles/cookie").Include("~/Scripts/cookiesamtykke.js"));

            // Noty JavaScript bundle
            bundles.Add(new ScriptBundle("~/bundles/noty").Include("~/Scripts/Noty/jquery.noty.js", "~/Scripts/Noty/layouts/topLeft.js", "~/Scripts/Noty/themes/default.js"));

            //Dawa
            //bundles.Add(new ScriptBundle("~/bundles/dawa").Include("~/Scripts/jQuery/jquery-ui*"));
            

			//QR bundle
			bundles.Add(new ScriptBundle("~/bundles/QR").Include("~/Scripts/QR/jquery.qrcode.min.js"));

			// Styles
			// The Kendo CSS bundle
			bundles.Add(new StyleBundle("~/bundles/Content/kendo").Include("~/Content/Css/Kendo/kendo.common.*", "~/Content/Css/Kendo/kendo.uniform.*"));

			

		    var domain = ConfigurationManager.AppSettings["FlytJordDomain"];

            //Sikre unik og genkendelig styling på testmiljø
            if (domain != null && (domain.ToLower().Contains("test") || domain.ToLower().Contains("localhost")))
		    {

		        // public page css
		        bundles.Add(new StyleBundle("~/bundles/Content/Css/Public").Include("~/Content/Css/Public/publicSite.css",
		            "~/Content/Css/Public/publicSite_test.css"));

		        // TjekEjendom page css
		        bundles.Add(
		            new StyleBundle("~/bundles/Content/Css/TjekEjendom").Include("~/Content/Css/Public/TjekEjendom.css",
		                "~/Content/Css/Public/publicSite_test.css"));

		        // Backend pages css
		        bundles.Add(new StyleBundle("~/bundles/Content/Css/Backend").Include("~/Content/Css/Backend/backendSite.css",
		            "~/Content/Css/Public/publicSite_test.css"));

                // logon page css
                bundles.Add(new StyleBundle("~/bundles/Content/Css/Login").Include("~/Content/Css/LoginSite.css", "~/Content/Css/Public/publicSite_test.css"));
		    }
		    else
		    {
                // public page css
                bundles.Add(new StyleBundle("~/bundles/Content/Css/Public").Include("~/Content/Css/Public/publicSite.css"));

                // TjekEjendom page css
                bundles.Add(
                    new StyleBundle("~/bundles/Content/Css/TjekEjendom").Include("~/Content/Css/Public/TjekEjendom.css"));

                // Backend pages css
                bundles.Add(new StyleBundle("~/bundles/Content/Css/Backend").Include("~/Content/Css/Backend/backendSite.css"));

                // logon page css
                bundles.Add(new StyleBundle("~/bundles/Content/Css/Login").Include("~/Content/Css/LoginSite.css"));
		    }
		    // Blanket pages css
			bundles.Add(new StyleBundle("~/bundles/Content/Css/PublicBlanket").Include("~/Content/Css/Public/Blanket.css"));

			// Common css
			bundles.Add(new StyleBundle("~/bundles/Content/Css/Common").Include("~/Content/Css/common.css"));
			bundles.Add(new StyleBundle("~/bundles/Content/Css/content").Include("~/Content/Css/content.css"));

			// Mobile CSS
			bundles.Add(
				new StyleBundle("~/bundles/Content/Css/Mobile").Include("~/Content/Css/jQueryMobile/jquery.mobile-{version}.css",
				                                                        "~/Content/Css/jQueryMobile/jquery.mobile.structure-{version}.css",
				                                                        "~/Content/Css/jQueryMobile/jquery.mobile.theme-{version}.css",
																																"~/Content/Css/Mobile/mobile.css"));
																																				//	"~/Content/Css/jQueryMobile/theme.css"));
			//"~/Content/Css/jQueryMobile/jquery.mobile.theme-{version}.css"));
			bundles.Add(new StyleBundle("~/bundles/Content/Css/BootstrapDatepicker").Include("~/Content/Css/datepicker.css"));

			//jQueryUI
			//bundles.Add(new StyleBundle("~/bundles/Content/Css/jQueryUI").Include("~/Content/Css/jQueryUI/themes/smoothness/jquery-ui.css","~/Content/Css/jQueryUI/themes/smoothness/jquery.ui.theme.css"));
			bundles.Add(new StyleBundle("~/bundles/Content/Css/jQueryUI").Include("~/Content/Css/jQueryUI/themes/smoothness/jquery-ui.css"));
      

			// Clear all items from the default ignore list to allow minified CSS and JavaScript files to be included in debug mode
			bundles.IgnoreList.Clear();

			// Add back the default ignore list rules sans the ones which affect minified files and debug mode
			bundles.IgnoreList.Ignore("*.intellisense.js");
			bundles.IgnoreList.Ignore("*-vsdoc.js");
			bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);

            if (domain != null && (domain.ToLower().Contains("test") || domain.ToLower().Contains("localhost")))
            BundleTable.EnableOptimizations = false;
		}
	}
}