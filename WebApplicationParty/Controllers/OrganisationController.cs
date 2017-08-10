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

        public async Task<IActionResult> Index(int id = 0)
        {
            if (id > 0)
            {
                var party = await _partyRespository.GetParty(id);
                var organisation = party.Organisation;

                // This should be mapped
                var model = new OrganisationViewModel
                {
                    PartyId = organisation.PartyId,
                    OrganisationName = organisation.OrganisationName,
                    TradingName = organisation.TradingName
                };

                return View(model);
            }

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
                PartyId = model.PartyId,
                OrganisationName = model.OrganisationName,
                TradingName = model.TradingName
            };

            if (model.PartyId > 0)
            {
                await _partyRespository.UpdateOrganisation(organisation);
            }
            else
            {
                await _partyRespository.AddOrganisation(organisation);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
