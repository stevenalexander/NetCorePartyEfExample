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

        public async Task<IActionResult> Index(int id = 0)
        {
            if (id > 0)
            {
                var party = await _partyRespository.GetParty(id);
                var person = party.Person;

                // This should be mapped
                var model = new PersonViewModel
                {
                    PartyId = person.PartyId,
                    FirstName = person.FirstName,
                    Surname = person.Surname,
                    DateOfBirth = person.DateOfBirth,
                    EmailAddress = person.EmailAddress
                };

                return View(model);
            }

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
                PartyId = model.PartyId,
                FirstName = model.FirstName,
                Surname = model.Surname,
                DateOfBirth = model.DateOfBirth,
                EmailAddress = model.EmailAddress
            };

            if (model.PartyId > 0)
            {
                await _partyRespository.UpdatePerson(person);
            }
            else
            {
                await _partyRespository.AddPerson(person);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
