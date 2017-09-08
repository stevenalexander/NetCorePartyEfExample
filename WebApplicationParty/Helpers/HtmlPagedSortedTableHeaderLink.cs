using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplicationParty.Models;

namespace WebApplicationParty.Helpers
{
    public static class HtmlPagedSortedTableHeaderLink
    {
        public static IHtmlContent PagedSortedTableHeaderLink(this IHtmlHelper htmlHelper, string text, string columnName, IPagedSortedViewModel model, string cssClasses = "")
        {
            bool isSortColumn = columnName.Equals(model.OrderColumn);
            bool newOrderAscending = !isSortColumn ? true : !model.OrderAscending;
            string sortIcon = isSortColumn ? model.OrderAscending ? "fa-sort-asc" : "fa-sort-desc" : string.Empty;
            string href = $"?start={model.Start}&length={model.Length}&orderColumn={columnName}&orderAscending={newOrderAscending}";

            return new HtmlString($"<a href=\"{href}\" class=\"{cssClasses}\">{text} <i class=\"fa fa-fw {sortIcon}\"></a>");
        }
    }
}
