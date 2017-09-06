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
using PartyData.Data;

namespace WebApplicationParty.UnitTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;
        private IPartyRespository _partyRespository;
        private IPagedSortedRepository<PartyWithRegistrationsResultItem> _pagedSortedRepository;

        [SetUp]
        public void SetUp()
        {
            _partyRespository = Substitute.For<IPartyRespository>();
            _pagedSortedRepository = Substitute.For<IPagedSortedRepository<PartyWithRegistrationsResultItem>>();
            _controller = new HomeController(_partyRespository, _pagedSortedRepository);
        }

        [Test]
        public async Task Index_GetsDataAndReturnsView()
        {
            var pagedSortedResult = new PagedSortedResult<PartyWithRegistrationsResultItem>()
            {
                recordsFiltered = 50,
                recordsTotal = 100,
                data = new List<PartyWithRegistrationsResultItem>(),
            };
            var services = new List<CustomService>();

            _pagedSortedRepository.GetPagedSortedResults(0, 10, "Name", true).Returns(pagedSortedResult);
            _partyRespository.GetCustomServices().Returns(services);

            var result = await _controller.Index();

            Assert.IsTrue(result is ViewResult);
            Assert.AreEqual(pagedSortedResult.recordsTotal, ((result as ViewResult).Model as HomeViewModel).RecordsTotal);
            Assert.AreEqual(pagedSortedResult.recordsFiltered, ((result as ViewResult).Model as HomeViewModel).RecordsFiltered);
            Assert.AreSame(pagedSortedResult.data, ((result as ViewResult).Model as HomeViewModel).Data);
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
        public async Task RemoveFromCustomService_RemovesPartyRegistrationAndReturnsRedirect()
        {
            var partyId = 4321;
            var customServiceId = 1234;

            var result = await _controller.RemoveFromCustomService(new HomeViewModel { PartyId = partyId, CustomServiceId = customServiceId });

            await _partyRespository.Received().RemovePartyFromCustomService(partyId, customServiceId);
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
