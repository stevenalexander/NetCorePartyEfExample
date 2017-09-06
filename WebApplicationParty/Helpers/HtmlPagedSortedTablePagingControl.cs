using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using WebApplicationParty.Models;

namespace WebApplicationParty.Helpers
{
    public static class HtmlPagedSortedTablePagingControl
    {
        public static IHtmlContent PagedSortedTablePagingControl(this IHtmlHelper htmlHelper, IPagedSortedViewModel model)
        {
            const string disabled = "class=\"disabled\"";

            var pageNumber = model.GetCurrentPageNumber();
            var pageCount = model.GetPageCount();

            var previousPageStart = model.Start - model.Length;
            previousPageStart = previousPageStart < 0 ? 0 : previousPageStart;
            var previousDisabled = pageNumber <= 1 ? disabled : string.Empty;

            var nextPageStart = model.Start + model.Length;
            var nextDisabled = pageNumber >= pageCount ? disabled : string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("<ul class=\"pagination\">");
            sb.AppendLine($"  <li {previousDisabled}><a href=\"{GetPageLink(previousPageStart, model)}\">&laquo;</a></li>");
            sb.AppendLine($"  <li class=\"active\"><a href=\"#\">{pageNumber}</a></li>");
            sb.AppendLine($"  <li {nextPageStart}><a href=\"{GetPageLink(nextPageStart, model)}\">&raquo;</a></li>");
            sb.AppendLine("</ul>");

            return new HtmlString(sb.ToString());
        }

        private static string GetPageLink(int start, IPagedSortedViewModel model)
        {
            return $"?start={start}&length={model.Length}&orderColumn={model.OrderColumn}&orderAscending={model.OrderAscending}";
        }
    }
}
