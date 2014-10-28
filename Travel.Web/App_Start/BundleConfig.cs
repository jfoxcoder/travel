using System.Web;
using System.Web.Optimization;
using Travel.Web.App_Start.Bundles.Upload;

namespace Travel.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // jquery
            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/jquery-{version}.js"));
            
            // slick slider (removed)
            //bundles.Add(new ScriptBundle("~/bundles/carousel")
            //       .Include("~/slick/slick.min.js", "~/Scripts/joseph/slick-view-destination-init.js"));

            // flash messages
            bundles.Add(new ScriptBundle("~/bundles/flash-messages")
                .Include("~/Scripts/joseph/flash-messages.js"));

            // country form
            bundles.Add(new ScriptBundle("~/bundles/country-form")
               .Include("~/Scripts/joseph/country-form.js"));

            // qtip tooltips
            bundles.Add(new ScriptBundle("~/bundles/qtip")
                .Include("~/Content/jquery.qtip.custom/jquery.qtip.min.js",
              "~/Scripts/joseph/tooltips.js"));

            // maximage (full-screen slider)
            bundles.Add(new ScriptBundle("~/bundles/maximage").Include(
                         "~/Content/MaxImage/jquery.cycle.all.js",
                         "~/Content/MaxImage/jquery.easing.1.3.js",
                         "~/Content/MaxImage/jquery.maximage.min.js",
                         "~/Scripts/Joseph/maximage-init.js"));


            // Used for file uploads
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jQuery.FileUpload/jqueryui/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
           
            // zurb foundation 5
            bundles.Add(Foundation.Scripts());

            // backload
            bundles.Add(Upload.Scripts());    


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

           
            bundles.Add(new StyleBundle("~/Content/css").Include(                
                "~/sass/Site.css",
                
                // foundation icons don't have an empty-star icon for the
                // wishlist so using custom icomoon icon font.
                //"~/Content/foundation-icons/foundation-icons.css",

                "~/Content/icomoon/style.css"));

           


            //bundles.Add(new StyleBundle("~/Content/carousel").Include("~/slick/slick.css"));
        }
    }
}
