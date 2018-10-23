using System.Web;
using System.Web.Optimization;

namespace Rcm.Site {
    public class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            //site
            bundles.Add(new ScriptBundle("~/bundles/global").Include(
                "~/Scripts/global.js",
                "~/Scripts/template.js"));
            bundles.Add(new StyleBundle("~/bundles/global/css").Include(
                "~/Content/global.css"));

            //ux widgets
            bundles.Add(new ScriptBundle("~/bundles/ux").Include(
                "~/Scripts/ux/label.js",
                "~/Scripts/ux/IFrame.js",
                "~/Scripts/ux/MultiCombo.js",
                "~/Scripts/ux/SingleCombo.js"));
            bundles.Add(new StyleBundle("~/bundles/ux/css").Include(
                "~/Content/themes/css/ux/global.css",
                "~/Content/themes/css/ux/icon.css",
                "~/Content/themes/css/ux/label.css",
                "~/Content/themes/css/ux/multicombo.css"));

            //component
            bundles.Add(new ScriptBundle("~/bundles/components").Include(
                "~/Scripts/components/StationTypeComponent.js",
                "~/Scripts/components/DeviceTypeComponent.js",
                "~/Scripts/components/AlarmLevelComponent.js"));
        }
    }
}