using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Travel.Web.App_Start.Bundles.Upload
{
    public static class Upload
    {
        
                
        public static Bundle Scripts()
        {
            return new ScriptBundle("~/bundles/upload").Include(

                "~/Scripts/jQuery.FileUpload/jqueryui/jquery.ui.widget.js",
                "~/Scripts/jQuery.FileUpload/tmpl.debug.js",
                "~/Scripts/jQuery.FileUpload/load-image.debug.js",
                "~/Scripts/jQuery.FileUpload/canvas-to-blob.debug.js",
                "~/Scripts/jQuery.FileUpload/bootstrap/bootstrap.debug.js",
                "~/Scripts/jQuery.FileUpload/bootstrap/bootstrap-image-gallery.debug.js",
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-process.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-image.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-audio.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-video.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-validate.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload-ui.js",
                
                // my upload code
                "~/Scripts/joseph/upload.js"
            );
        }
         /*bundles.Add(new StyleBundle("~/bundles/fileupload/bootstrap/BasicPlusUI/css").Include(
                clientFiles.Styles + "bootstrap/bootstrap.debug.css",
                clientFiles.Styles + "bootstrap/bootstrap-responsive.debug.css",
                clientFiles.Styles + "bootstrap/bootstrap-image-gallery.debug.css",
                clientFiles.Styles + "jquery.fileupload-ui.css")
            );*/

        public static Bundle Styles()
        {
            return new StyleBundle("~/bundles/upload").Include(
                "~/Content/FileUpload/css/bootstrap/bootstrap.debug.css",
                "~/Content/FileUpload/css/bootstrap/bootstrap-responsive.debug.css",
                "~/Content/FileUpload/css/bootstrap/bootstrap-image-gallery.debug.css",
                "~/Content/FileUpload/css/jquery.fileupload-ui.css"
            );
        }
    }
}