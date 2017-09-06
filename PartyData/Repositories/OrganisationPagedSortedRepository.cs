using System.Collections.Generic;
using System.Linq;
using PartyData.Data;

namespace PartyData.Repositories
{
    public class OrganisationPagedSortedRepository : AbstractPagedSortedRepository<OrganisationResultItem>
    {
        private PartyDbContext _partyDbContext;

        public OrganisationPagedSortedRepository(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        protected override IQueryable<OrganisationResultItem> GetQuery()
        {
            return from p in _partyDbContext.Parties
                   join o in _partyDbContext.Organisations on p.PartyId equals o.PartyId
                   select new OrganisationResultItem { PartyId = p.PartyId, Name = p.Name, TradingName = o.TradingName, DateCreated = p.DateCreated };
        }

        protected override IQueryable<OrganisationResultItem> GetSortedWhereQuery(IQueryable<OrganisationResultItem> whereQuery, string orderColumn, bool orderAscending)
        {
            switch (orderColumn)
            {
                case "Name": return orderAscending ? whereQuery.OrderBy(x => x.Name) : whereQuery.OrderByDescending(x => x.Name);
                case "TradingName": return orderAscending ? whereQuery.OrderBy(x => x.TradingName) : whereQuery.OrderByDescending(x => x.TradingName);
                case "DateCreated": return orderAscending ? whereQuery.OrderBy(x => x.DateCreated) : whereQuery.OrderByDescending(x => x.DateCreated);
                default: return whereQuery;
            }
        }
    }
}
