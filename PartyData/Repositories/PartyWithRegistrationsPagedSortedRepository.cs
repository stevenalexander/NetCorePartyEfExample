using System.Collections.Generic;
using System.Linq;
using PartyData.Data;
using System;

namespace PartyData.Repositories
{
    public class PartyWithRegistrationsPagedSortedRepository : AbstractPagedSortedRepository<PartyWithRegistrationsResultItem>
    {
        private PartyDbContext _partyDbContext;

        public PartyWithRegistrationsPagedSortedRepository(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        protected override IQueryable<PartyWithRegistrationsResultItem> GetQuery()
        {
            return from party in _partyDbContext.Parties
                   join registration in _partyDbContext.PartyCustomServiceRegistrations on party.PartyId equals registration.PartyId into registrations
                   select new PartyWithRegistrationsResultItem
                   {
                       PartyId = party.PartyId,
                       Name = party.Name,
                       ActiveRegistrationCustomServiceIds = registrations.Where(x => x.RegistrationStatus).Select(x => x.CustomServiceId)
                   };
        }

        protected override IQueryable<PartyWithRegistrationsResultItem> GetRecordsTotalQuery(IQueryable<PartyWithRegistrationsResultItem> query)
        {
            return GetQueryWithoutGroupJoin();
        }

        protected override IQueryable<PartyWithRegistrationsResultItem> GetRecordsFilteredQuery(IQueryable<PartyWithRegistrationsResultItem> whereQuery)
        {
            return GetQueryWithoutGroupJoin();
        }

        protected override IQueryable<PartyWithRegistrationsResultItem> GetSortedWhereQuery(IQueryable<PartyWithRegistrationsResultItem> whereQuery, string orderColumn, bool orderAscending)
        {
            switch (orderColumn)
            {
                case "Name": return orderAscending ? whereQuery.OrderBy(x => x.Name) : whereQuery.OrderByDescending(x => x.Name);
                default: return whereQuery;
            }
        }

        private IQueryable<PartyWithRegistrationsResultItem> GetQueryWithoutGroupJoin()
        {
            return from party in _partyDbContext.Parties
                   select new PartyWithRegistrationsResultItem
                   {
                       PartyId = party.PartyId,
                       Name = party.Name,
                       ActiveRegistrationCustomServiceIds = null,
                   };
        }
    }
}
