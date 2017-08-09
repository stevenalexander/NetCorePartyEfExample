using Microsoft.AspNetCore.Mvc;
using PartyData;
using PartyData.Entities;
using System.Threading.Tasks;
using WebApplicationParty.Models;

namespace WebApplicationParty.Controllers
{
    public class PersonController : Controller
    {
        private PartyDbContext _partyDbContext;

        public PersonController(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PersonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var person = new Person
            {
                FirstName = model.FirstName,
                Surname = model.Surname,
                DateOfBirth = model.DateOfBirth,
                EmailAddress = model.EmailAddress,
                Party = new Party(string.Format("{0} {1}", model.FirstName, model.Surname))
            };

            _partyDbContext.Add(person);
            await _partyDbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
