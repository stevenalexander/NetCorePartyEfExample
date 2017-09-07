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
            var model = await GetPagedSortedResultsAsViewModel(0, start, length, orderColumn, orderAscending);

            return View(model);
        }

        [HttpGet]
        public IActionResult Datatable()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> DatatableJson(int draw = 0, int start = 0, int length = 10)
        {
            var isAscending = Request.Query["order[0][dir]"] == "asc";
            int columnIdentifier = Convert.ToInt32(Request.Query["order[0][column]"]);
            string orderColumnName = GetColumnName(columnIdentifier);

            var model = await GetPagedSortedResultsAsViewModel(draw, start, length, orderColumnName, isAscending);

            return Json(model);
        }

        private async Task<PagedSortedViewModel<OrganisationResultItem>> GetPagedSortedResultsAsViewModel(int draw, int start, int length, string orderColumn, bool orderAscending)
        {
            var result = await _pagedSortedRepository.GetPagedSortedResults(start, length, orderColumn, orderAscending);

            return new PagedSortedViewModel<OrganisationResultItem>
            {
                Draw = draw,
                Start = start,
                Length = length,
                OrderAscending = orderAscending,
                OrderColumn = orderColumn,
                RecordsFiltered = result.recordsFiltered,
                RecordsTotal = result.recordsTotal,
                Data = result.data,
            };
        }

        private string GetColumnName(int columnIdentifier)
        {
            switch(columnIdentifier)
            {
                case 0: return "Name";
                case 1: return "TradingName";
                case 2: return "DateCreated";
                default: return "Name";
	        }

        }
    }
}
