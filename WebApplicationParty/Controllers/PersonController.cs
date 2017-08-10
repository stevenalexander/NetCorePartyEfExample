using Microsoft.AspNetCore.Mvc;
using PartyData;
using PartyData.Entities;
using PartyData.Repositories;
using System.Threading.Tasks;
using WebApplicationParty.Models;

namespace WebApplicationParty.Controllers
{
    public class PersonController : Controller
    {
        private IPartyRespository _partyRespository;

        public PersonController(IPartyRespository partyRespository)
        {
            _partyRespository = partyRespository;
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

            await _partyRespository.AddPerson(person);

            return RedirectToAction("Index", "Home");
        }
    }
}
