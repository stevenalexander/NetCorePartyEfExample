using Microsoft.AspNetCore.Mvc;
using PartyData;
using PartyData.Entities;
using PartyData.Repositories;
using System.Threading.Tasks;
using WebApplicationParty.Models;

namespace WebApplicationParty.Controllers
{
    public class OrganisationController : Controller
    {
        private IPartyRespository _partyRespository;

        public OrganisationController(IPartyRespository partyRespository)
        {
            _partyRespository = partyRespository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var organisation = new Organisation
            {
                OrganisationName = model.OrganisationName,
                TradingName = model.TradingName,
                Party = new Party(model.OrganisationName)
            };

            await _partyRespository.AddOrganisation(organisation);

            return RedirectToAction("Index", "Home");
        }
    }
}
