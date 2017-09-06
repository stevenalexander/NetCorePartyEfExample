using Microsoft.AspNetCore.Mvc;
using PartyData.Data;
using PartyData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationParty.Models;

namespace WebApplicationParty.Controllers
{
    public class OrganisationPagedSortedTableController : Controller
    {
        private IPagedSortedRepository<OrganisationResultItem> _pagedSortedRepository;

        public OrganisationPagedSortedTableController(IPagedSortedRepository<OrganisationResultItem> pagedSortedRepository)
        {
            _pagedSortedRepository = pagedSortedRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int start = 0, int length = 10, string orderColumn = "Name", bool orderAscending = true)
        {
            var result = await _pagedSortedRepository.GetPagedSortedResults(start, length, orderColumn, orderAscending);

            var model = new PagedSortedViewModel<OrganisationResultItem>
            {
                Start = start,
                Length = length,
                OrderAscending = orderAscending,
                OrderColumn = orderColumn,
                RecordsFiltered = result.recordsFiltered,
                RecordsTotal = result.recordsTotal,
                Data = result.data,
            };

            return View(model);
        }
    }
}
