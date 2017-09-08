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
    public class PersonPagedSortedTableController : Controller
    {
        private IPagedSortedRepository<PersonResultItem> _pagedSortedRepository;

        public PersonPagedSortedTableController(IPagedSortedRepository<PersonResultItem> pagedSortedRepository)
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
        public async Task<JsonResult> DatatableJson(int draw = 0, int start = 0, int length = 10)
        {
            var isAscending = Request.Query["order[0][dir]"] == "asc";
            int columnIdentifier = Convert.ToInt32(Request.Query["order[0][column]"]);
            string orderColumnName = GetColumnName(columnIdentifier);

            var model = await GetPagedSortedResultsAsViewModel(draw, start, length, orderColumnName, isAscending);

            return Json(model);
        }

        private async Task<PagedSortedViewModel<PersonResultItem>> GetPagedSortedResultsAsViewModel(int draw, int start, int length, string orderColumn, bool orderAscending)
        {
            var result = await _pagedSortedRepository.GetPagedSortedResults(start, length, orderColumn, orderAscending);

            return new PagedSortedViewModel<PersonResultItem>
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
            switch (columnIdentifier)
            {
                case 0: return "Name";
                case 1: return "EmailAddress";
                case 2: return "DateOfBirth";
                case 3: return "DateCreated";
                default: return "Name";
            }

        }
    }
}
