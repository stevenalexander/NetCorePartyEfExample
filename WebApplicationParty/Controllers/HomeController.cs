using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyData;
using WebApplicationParty.Models;
using PartyData.Entities;
using PartyData.Repositories;

namespace WebApplicationParty.Controllers
{
    public class HomeController : Controller
    {
        private IPartyRespository _partyRespository;

        public HomeController(IPartyRespository partyRespository)
        {
            _partyRespository = partyRespository;
        }

        public async Task<IActionResult> Index()
        {
            var parties = await _partyRespository.GetPartiesWithRegistrations();
            var services = await _partyRespository.GetCustomServices();

            var model = new HomeViewModel() { Parties = parties, Services = services };

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
