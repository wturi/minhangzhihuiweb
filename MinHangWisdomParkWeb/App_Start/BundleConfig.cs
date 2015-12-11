using System.Web;
using System.Web.Optimization;

namespace MinHangWisdomParkWeb
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/plugins/jQuery/jQuery-2.1.4.min.js",
                        "~/bootstrap/js/bootstrap.min.js",
                        "~/plugins/morris/morris.min.js",
                        "~/plugins/sparkline/jquery.sparkline.min.js",
                        "~/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                        "~/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                        "~/plugins/knob/jquery.knob.js",
                        "~/plugins/daterangepicker/daterangepicker.js",
                        "~/plugins/datepicker/bootstrap-datepicker.js",
                        "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                        "~/plugins/slimScroll/jquery.slimscroll.min.js",
                        "~/plugins/fastclick/fastclick.min.js",
                        "~/dist/js/app.min.js",
                        "~/dist/js/pages/dashboard.js",
                        "~/dist/js/demo.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/bootstrap/css/bootstrap.min.css",
                "~/dist/css/AdminLTE.min.css",
                "~/dist/css/skins/_all-skins.min.css",
                "~/plugins/iCheck/flat/blue.css",
                "~/plugins/morris/morris.css",
                "~/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
                "~/plugins/datepicker/datepicker3.css",
                "~/plugins/daterangepicker/daterangepicker-bs3.css",
                "~/plugins/bootstrap-wysihtml5/bootstrap3-wysihtml5.min.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}