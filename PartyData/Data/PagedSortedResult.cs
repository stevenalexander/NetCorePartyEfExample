using System.Collections.Generic;

namespace PartyData.Data
{
    public class PagedSortedResult<TData>
    {
        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public IEnumerable<TData> data { get; set; }

        public string error { get; set; }
    }
}
