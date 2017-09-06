using System;
using System.Collections.Generic;

namespace WebApplicationParty.Models
{
    public class PagedSortedViewModel<TData> : IPagedSortedViewModel
    {
        public int Start { get; set; }

        public int Length { get; set; }

        public string OrderColumn { get; set; }

        public bool OrderAscending { get; set; }

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public IEnumerable<TData> Data { get; set; }

        public int GetCurrentPageNumber()
        {
            return (int)Math.Floor(Start / (decimal)Length) + 1;
        }

        public int GetPageCount()
        {
            return (int)Math.Floor(RecordsFiltered / (decimal)Length) + 1;
        }
    }
}