using System.Web;
using System.Web.Optimization;

namespace BeverageManagement
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            var cssPath = "~/Content/css/";
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-table.js",
                      "~/Scripts/bootstrap-table-filter.js",
                      "~/Scripts/bootstrap-table-export.js",
                       "~/Content/TinyMce/tinymce.min.js",
                       "~/Scripts/moment.js",
                       "~/Scripts/bootstrap-datetimepicker.min.js",
                      "~/Scripts/Custom.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      cssPath + "bootstrap.min.css",
                      cssPath + "bootstrap-table.css",
                      cssPath + "font-awesome.css",
                      cssPath + "custom-style.css",
                      cssPath + "site.css",
                      cssPath + "bootstrap-datetimepicker.min.css",
                      cssPath + "bootstrap-datetimepicker-override.css",
                      cssPath + "jquery-ui.min.css"));

#if DEBUG
            BundleTable.EnableOptimizations = false;
            bundles.UseCdn = false;
#else
            BundleTable.EnableOptimizations = true;
            bundles.UseCdn = true;
#endif


        }
    }
}
