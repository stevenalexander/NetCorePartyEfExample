using Microsoft.AspNetCore.Mvc;
using PartyData;
using PartyData.Entities;
using PartyData.Repositories;
using System.Threading.Tasks;
using WebApplicationParty.Models;

namespace WebApplicationParty.Controllers
{
    public class PartyController : Controller
    {
        private IPartyRespository _partyRespository;

        public PartyController(IPartyRespository partyRespository)
        {
            _partyRespository = partyRespository;
        }

        public async Task<IActionResult> Index(int id = 0)
        {
            if (id > 0)
            {
                var party = await _partyRespository.GetParty(id);

                if (party.Organisation != null)
                {
                    var organisation = party.Organisation;

                    var model = new OrganisationViewModel
                    {
                        PartyId = organisation.PartyId,
                        OrganisationName = organisation.OrganisationName,
                        TradingName = organisation.TradingName
                    };

                    return View("Organisation", model);
                }
                else
                {
                    var person = party.Person;

                    var model = new PersonViewModel
                    {
                        PartyId = person.PartyId,
                        FirstName = person.FirstName,
                        Surname = person.Surname,
                        DateOfBirth = person.DateOfBirth,
                        EmailAddress = person.EmailAddress
                    };

                    return View("Person", model);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePerson(PersonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Person", model);
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

        [HttpPost]
        public async Task<IActionResult> UpdateOrganisation(OrganisationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Organisation", model);
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
