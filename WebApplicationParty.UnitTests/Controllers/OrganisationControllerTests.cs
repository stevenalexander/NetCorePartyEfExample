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
    public class OrganisationControllerTests
    {
        private OrganisationController _controller;
        private IPartyRespository _partyRespository;

        [SetUp]
        public void SetUp()
        {
            _partyRespository = Substitute.For<IPartyRespository>();
            _controller = new OrganisationController(_partyRespository);
        }

        [Test]
        public void Index_ReturnsView()
        {
            var result = _controller.Index();

            Assert.IsTrue(result is ViewResult);
        }

        [Test]
        public async Task Index_PostAddsOrganisationAndReturnsRedirect()
        {
            var organisationName = "org1";
            var tradingName = "trading1";

            var result = await _controller.Index(new OrganisationViewModel { OrganisationName = organisationName, TradingName = tradingName });

            await _partyRespository.Received().AddOrganisation(Arg.Is<Organisation>(o => o.OrganisationName == organisationName && o.TradingName == tradingName));
            Assert.IsTrue(result is RedirectToActionResult);
        }

        [Test]
        public async Task Index_PostWithInvalidModelReturnsView()
        {
            var model = new OrganisationViewModel();
            _controller.ModelState.AddModelError("", "invalid model");

            var result = await _controller.Index(model);

            Assert.IsTrue(result is ViewResult);
            Assert.AreSame(model, (result as ViewResult).Model);
        }
    }
}
