using System;
using NSubstitute;
using NUnit.Framework;
using WebApplicationParty.Controllers;
using PartyData.Repositories;
using PartyData.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplicationParty.Models;
using System.Threading.Tasks;

namespace WebApplicationParty.UnitTests.Controllers
{
    [TestFixture]
    public class PartyControllerTests
    {
        private PartyController _controller;
        private IPartyRespository _partyRespository;

        [SetUp]
        public void SetUp()
        {
            _partyRespository = Substitute.For<IPartyRespository>();
            _controller = new PartyController(_partyRespository);
        }

        [Test]
        public async Task Index_WithoutIdReturnsRedirect()
        {
            var result = await _controller.Index();

            Assert.IsTrue(result is RedirectToActionResult);
        }

        [Test]
        public async Task Index_WithIdGetsDataPersonAndReturnsPersonView()
        {
            var party = new Party { Person = new Person() };
            _partyRespository.GetParty(1).Returns(party);

            var result = await _controller.Index(1);

            await _partyRespository.Received().GetParty(1);
            Assert.IsTrue(result is ViewResult);
            Assert.AreEqual("Person", (result as ViewResult).ViewName);
        }

        [Test]
        public async Task Index_WithIdGetsDataOrganisationAndReturnsOrganisationView()
        {
            var party = new Party { Organisation = new Organisation() };
            _partyRespository.GetParty(1).Returns(party);

            var result = await _controller.Index(1);

            await _partyRespository.Received().GetParty(1);
            Assert.IsTrue(result is ViewResult);
            Assert.AreEqual("Organisation", (result as ViewResult).ViewName);
        }

        [Test]
        public async Task Index_UpdatePersonPostAddsPersonAndReturnsRedirect()
        {
            var firstName = "tim";
            var surname = "test";
            var dateOfBirth = new DateTime(2000,1,1);
            var emailAddress = "test@test.com";

            var result = await _controller.UpdatePerson(new PersonViewModel { FirstName = firstName, Surname = surname, DateOfBirth = dateOfBirth, EmailAddress = emailAddress });

            await _partyRespository.Received().AddPerson(Arg.Is<Person>(o =>
                o.FirstName == firstName &&
                o.Surname == surname &&
                o.DateOfBirth == dateOfBirth &&
                o.EmailAddress == emailAddress));

            Assert.IsTrue(result is RedirectToActionResult);
        }

        [Test]
        public async Task Index_UpdatePersonPostWithInvalidModelReturnsView()
        {
            var model = new PersonViewModel();
            _controller.ModelState.AddModelError("", "invalid model");

            var result = await _controller.UpdatePerson(model);

            Assert.IsTrue(result is ViewResult);
            Assert.AreSame(model, (result as ViewResult).Model);
        }

        [Test]
        public async Task Index_UpdateOrganisationPostAddsOrganisationAndReturnsRedirect()
        {
            var organisationName = "org1";
            var tradingName = "trading1";

            var result = await _controller.UpdateOrganisation(new OrganisationViewModel { OrganisationName = organisationName, TradingName = tradingName });

            await _partyRespository.Received().AddOrganisation(Arg.Is<Organisation>(o => o.OrganisationName == organisationName && o.TradingName == tradingName));
            Assert.IsTrue(result is RedirectToActionResult);
        }

        [Test]
        public async Task Index_UpdateOrganisationPostWithInvalidModelReturnsView()
        {
            var model = new OrganisationViewModel();
            _controller.ModelState.AddModelError("", "invalid model");

            var result = await _controller.UpdateOrganisation(model);

            Assert.IsTrue(result is ViewResult);
            Assert.AreSame(model, (result as ViewResult).Model);
        }
    }
}
