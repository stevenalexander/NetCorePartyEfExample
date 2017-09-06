namespace WebApplicationParty.Models
{
    public interface IPagedSortedViewModel
    {
        int Length { get; set; }
        bool OrderAscending { get; set; }
        string OrderColumn { get; set; }
        int RecordsFiltered { get; set; }
        int RecordsTotal { get; set; }
        int Start { get; set; }

        int GetCurrentPageNumber();

        int GetPageCount();
    }
}