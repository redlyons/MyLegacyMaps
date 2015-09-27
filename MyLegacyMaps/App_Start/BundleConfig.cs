using System.Web;
using System.Web.Optimization;

namespace MyLegacyMaps
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/scripts/jquery-{version}.js"
                        ,"~/scripts/masonry.pkgd.min.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                       "~/scripts/jquery-ui-{version}.js"
                       , "~/scripts/jquery.ui.touch-punch.min.js"
                       ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/scripts/bootstrap.js",
                      "~/scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/mlm").Include(
                      "~/scripts/mlm/mlm-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/mlm/mlm.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/PagedList.css",
                      "~/Content/zocial/zocial.css"));

           
        }
    }
}
