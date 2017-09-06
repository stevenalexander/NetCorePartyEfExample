using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PartyData.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PartyData.Repositories
{
    public abstract class AbstractPagedSortedRepository<TResultItem> : IPagedSortedRepository<TResultItem>
    {
        public async Task<PagedSortedResult<TResultItem>> GetPagedSortedResults(int start, int length, string orderColumn, bool orderAscending)
        {
            var innerJoinQuery = GetQuery();

            var recordsTotal = await GetRecordsTotalQuery(innerJoinQuery).CountAsync();

            var whereQuery = GetWhereQuery(innerJoinQuery);

            var recordsFiltered = await GetRecordsFilteredQuery(whereQuery).CountAsync();

            var sortedWhereQuery = GetSortedWhereQuery(whereQuery, orderColumn, orderAscending);

            var pagedSortedWhereQuery = sortedWhereQuery.Skip(start).Take(length);

            var data = await pagedSortedWhereQuery.ToListAsync();

            return new PagedSortedResult<TResultItem>
            {
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered,
                data = data,
            };
        }

        /// <summary>
        /// Get the query that will return the ResultItems, including any joins
        /// </summary>
        /// <returns>query</returns>
        protected abstract IQueryable<TResultItem> GetQuery();

        /// <summary>
        /// Get the records total query.
        /// Default returns base query.
        /// Needs to exclude group joins as this will break count call.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual IQueryable<TResultItem> GetRecordsTotalQuery(IQueryable<TResultItem> query)
        {
            return query;
        }

        /// <summary>
        /// Get the records filtered query.
        /// Default returns base query.
        /// Needs to exclude group joins as this will break count call.
        /// </summary>
        /// <param name="whereQuery"></param>
        /// <returns></returns>
        protected virtual IQueryable<TResultItem> GetRecordsFilteredQuery(IQueryable<TResultItem> whereQuery)
        {
            return whereQuery;
        }

        /// <summary>
        /// Apply filters and conditions to the base query.
        /// Default returns base query.
        /// </summary>
        /// <param name="query">base query</param>
        /// <returns>where query</returns>
        protected virtual IQueryable<TResultItem> GetWhereQuery(IQueryable<TResultItem> query)
        {
            return query;
        }

        /// <summary>
        /// Apply sorting to the where query
        /// </summary>
        /// <param name="whereQuery">where query</param>
        /// <param name="orderColumn">order column</param>
        /// <param name="orderAscending">is order ascending</param>
        /// <returns>sorted where query</returns>
        protected abstract IQueryable<TResultItem> GetSortedWhereQuery(IQueryable<TResultItem> whereQuery, string orderColumn, bool orderAscending);
    }
}
