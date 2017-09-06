using System.Threading.Tasks;
using PartyData.Data;

namespace PartyData.Repositories
{
    public interface IPagedSortedRepository<TResultItem>
    {
        Task<PagedSortedResult<TResultItem>> GetPagedSortedResults(int start, int length, string orderColumn, bool orderAscending);
    }
}