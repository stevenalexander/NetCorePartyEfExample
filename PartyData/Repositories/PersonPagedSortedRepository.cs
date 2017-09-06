using System.Collections.Generic;
using System.Linq;
using PartyData.Data;
using System;

namespace PartyData.Repositories
{
    public class PersonPagedSortedRepository : AbstractPagedSortedRepository<PersonResultItem>
    {
        private PartyDbContext _partyDbContext;

        public PersonPagedSortedRepository(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        protected override IQueryable<PersonResultItem> GetQuery()
        {
            return from p in _partyDbContext.Parties
                   join o in _partyDbContext.Persons on p.PartyId equals o.PartyId
                   select new PersonResultItem { PartyId = p.PartyId, Name = p.Name, EmailAddress = o.EmailAddress, DateOfBirth = o.DateOfBirth, DateCreated = p.DateCreated };
        }

        protected override IQueryable<PersonResultItem> GetWhereQuery(IQueryable<PersonResultItem> query)
        {
            return query.Where(x => x.DateOfBirth < new DateTime(1980, 1, 1)); // Example filter
        }

        protected override IQueryable<PersonResultItem> GetSortedWhereQuery(IQueryable<PersonResultItem> whereQuery, string orderColumn, bool orderAscending)
        {
            switch (orderColumn)
            {
                case "Name": return orderAscending ? whereQuery.OrderBy(x => x.Name) : whereQuery.OrderByDescending(x => x.Name);
                case "EmailAddress": return orderAscending ? whereQuery.OrderBy(x => x.EmailAddress) : whereQuery.OrderByDescending(x => x.EmailAddress);
                case "DateOfBirth": return orderAscending ? whereQuery.OrderBy(x => x.DateOfBirth) : whereQuery.OrderByDescending(x => x.DateOfBirth);
                case "DateCreated": return orderAscending ? whereQuery.OrderBy(x => x.DateCreated) : whereQuery.OrderByDescending(x => x.DateCreated);
                default: return whereQuery;
            }
        }
    }
}
