using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyData;
using WebApplicationParty.Models;
using PartyData.Entities;
using PartyData.Repositories;
using PartyData.Data;

namespace WebApplicationParty.Controllers
{
    public class HomeController : Controller
    {
        private IPartyRespository _partyRespository;
        private IPagedSortedRepository<PartyWithRegistrationsResultItem> _pagedSortedRepository;

        public HomeController(IPartyRespository partyRespository, IPagedSortedRepository<PartyWithRegistrationsResultItem> pagedSortedRepository)
        {
            _partyRespository = partyRespository;
            _pagedSortedRepository = pagedSortedRepository;
        }

        public async Task<IActionResult> Index(int start = 0, int length = 10, string orderColumn = "Name", bool orderAscending = true)
        {
            var result = await _pagedSortedRepository.GetPagedSortedResults(start, length, orderColumn, orderAscending);

            var services = await _partyRespository.GetCustomServices();

            var model = new HomeViewModel()
            {
                Start = start,
                Length = length,
                OrderAscending = orderAscending,
                OrderColumn = orderColumn,
                RecordsFiltered = result.recordsFiltered,
                RecordsTotal = result.recordsTotal,
                Data = result.data,
                Services = services,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCustomService(HomeViewModel model)
        {
            await _partyRespository.RegisterPartyWithService(model.PartyId, model.CustomServiceId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCustomService(HomeViewModel model)
        {
            await _partyRespository.RemovePartyFromCustomService(model.PartyId, model.CustomServiceId);

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
