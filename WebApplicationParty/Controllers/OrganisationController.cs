using Microsoft.AspNetCore.Mvc;
using PartyData;
using PartyData.Entities;
using System.Threading.Tasks;
using WebApplicationParty.Models;

namespace WebApplicationParty.Controllers
{
    public class OrganisationController : Controller
    {
        private PartyDbContext _partyDbContext;

        public OrganisationController(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
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

            _partyDbContext.Add(organisation);
            await _partyDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
