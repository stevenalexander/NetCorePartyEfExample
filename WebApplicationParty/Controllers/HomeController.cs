using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyData;
using WebApplicationParty.Models;
using PartyData.Entities;

namespace WebApplicationParty.Controllers
{
    public class HomeController : Controller
    {
        private PartyDbContext _partyDbContext;

        public HomeController(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var parties = await _partyDbContext.Parties.Include(p => p.CustomServiceRegistrations).ToListAsync();
            var services = await _partyDbContext.CustomServices.ToListAsync();
            var model = new HomeViewModel() { Parties = parties, Services = services };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCustomService(HomeViewModel model)
        {
            if (model.CustomServiceId > 0 && model.PartyId > 0)
            {
                var existingRegistrations = await _partyDbContext.PartyCustomServiceRegistrations.Where(r => r.PartyId == model.PartyId).ToListAsync();

                if (!existingRegistrations.Exists(r => r.CustomServiceId == model.CustomServiceId))
                {
                    var newRegistration = new PartyCustomServiceRegistration
                    {
                        PartyId = model.PartyId,
                        CustomServiceId = model.CustomServiceId,
                        RegistrationStatus = true
                    };

                    _partyDbContext.Add(newRegistration);
                    await _partyDbContext.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
