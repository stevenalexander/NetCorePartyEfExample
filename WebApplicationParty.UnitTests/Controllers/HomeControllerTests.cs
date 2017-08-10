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
    public class HomeControllerTests
    {
        private HomeController _controller;
        private IPartyRespository _partyRespository;

        [SetUp]
        public void SetUp()
        {
            _partyRespository = Substitute.For<IPartyRespository>();
            _controller = new HomeController(_partyRespository);
        }

        [Test]
        public async Task Index_GetsDataAndReturnsView()
        {
            var parties = new List<Party>();
            var services = new List<CustomService>();

            _partyRespository.GetPartiesWithRegistrations().Returns(parties);
            _partyRespository.GetCustomServices().Returns(services);

            var result = await _controller.Index();

            Assert.IsTrue(result is ViewResult);
            Assert.AreSame(parties, ((result as ViewResult).Model as HomeViewModel).Parties);
            Assert.AreSame(services, ((result as ViewResult).Model as HomeViewModel).Services);
        }

        [Test]
        public async Task AddToCustomService_RegistersPartyAndReturnsRedirect()
        {
            var partyId = 4321;
            var customServiceId = 1234;

            var result = await _controller.AddToCustomService(new HomeViewModel { PartyId = partyId, CustomServiceId = customServiceId });

            await _partyRespository.Received().RegisterPartyWithService(partyId, customServiceId);
            Assert.IsTrue(result is RedirectToActionResult);
        }

        [Test]
        public void Error_ReturnsView()
        {
            var result = _controller.Error();

            Assert.IsTrue(result is ViewResult);
        }
    }
}
