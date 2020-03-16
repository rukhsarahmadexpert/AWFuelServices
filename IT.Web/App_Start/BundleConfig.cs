using System.Web;
using System.Web.Optimization;

namespace IT.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/assets/jquery/jquery-2.1.1.min.js",                 
                        "~/Assets/assets/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Assets/assets/jquery-cookie/jquery.cookie.js",
                        "~/Assets/assets/flot/jquery.flot.js",
                        "~/Assets/assets/flot/jquery.flot.resize.js",
                        "~/Assets/assets/flot/jquery.flot.pie.js",
                        "~/Assets/assets/flot/jquery.flot.stack.js",
                        "~/Assets/assets/flot/jquery.flot.crosshair.js",
                        "~/Assets/assets/flot/jquery.flot.tooltip.min.js",
                        "~/Assets/assets/sparkline/jquery.sparkline.min.js",
                        "~/Scripts/js/data-table/jquery.dataTables.min.js",
                        "~/Scripts/js/data-table/data-table-act.js",
                        "~/Assets/assets/data-tables/bootstrap3/dataTables.bootstrap.js",
                        "~/Scripts/Common-Scripts/common-script.js",
                        "~/Scripts/Common-Scripts/massageAlert.js",
                        "~/Scripts/sweetalart/sweetalert.min.js",
                        "~/Assets/js/flaty.js",
                        "~/Assets/js/flaty-demo-codes.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Assets/assets/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Assets/assets/bootstrap/css/bootstrap.min.css",
                      "~/Assets/assets/font-awesome/css/font-awesome.min.css",
                       "~/Content/css/jquery.dataTables.min.css",
                       "~/Scripts/sweetalart/sweetalert.css",
                      "~/Assets/css/flaty.css",
                      "~/Assets/css/flaty-responsive.css"                     
                      ));
        }
    }
}
