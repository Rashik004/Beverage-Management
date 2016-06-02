using System.Web;
using System.Web.Mvc;

namespace BeverageManagement.Helpers {
    public static class HtmlHelpers {
        public static void SetPageId(this HtmlHelper helper, string id) {
            if (!string.IsNullOrEmpty(id)) {
                id = "id=\"" + id + "\"";
            };
            helper.ViewBag.pageId = new MvcHtmlString (id);
        }

    }
}