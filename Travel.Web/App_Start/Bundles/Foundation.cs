using System.Web.Optimization;

namespace Travel.Web
{
    public static class Foundation
    {
        public static Bundle Scripts()
        {

            //     "~/Scripts/jQuery.FileUpload/jqueryui/jquery.ui.widget.js",
            
            return new ScriptBundle("~/bundles/foundation").Include(
                      "~/Scripts/foundation/fastclick.js",
                      "~/Scripts/foundation/jquery.cookie.js",
                        "~/Scripts/foundation/foundation.js",
                        "~/Scripts/foundation/foundation.abide.js",
                        //"~/Scripts/foundation/foundation.accordion.js",
                        "~/Scripts/foundation/foundation.alert.js",
                        "~/Scripts/foundation/foundation.clearing.js",
                        "~/Scripts/foundation/foundation.dropdown.js",
                        //"~/Scripts/foundation/foundation.equalizer.js",
                        "~/Scripts/foundation/foundation.interchange.js",
                        //"~/Scripts/foundation/foundation.joyride.js",
                        //"~/Scripts/foundation/foundation.magellan.js",
                        "~/Scripts/foundation/foundation.offcanvas.js",
                        //"~/Scripts/foundation/foundation.orbit.js",
                        //"~/Scripts/foundation/foundation.reveal.js",
                        //"~/Scripts/foundation/foundation.slider.js",
                        //"~/Scripts/foundation/foundation.tab.js",
                        //"~/Scripts/foundation/foundation.tooltip.js",
                        "~/Scripts/foundation/foundation.topbar.js",
                        "~/Scripts/foundation/foundation-init.js");
        }
    }
}