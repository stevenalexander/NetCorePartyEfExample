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
    public class PersonControllerTests
    {
        private PersonController _controller;
        private IPartyRespository _partyRespository;

        [SetUp]
        public void SetUp()
        {
            _partyRespository = Substitute.For<IPartyRespository>();
            _controller = new PersonController(_partyRespository);
        }

        [Test]
        public void Index_ReturnsView()
        {
            var result = _controller.Index();

            Assert.IsTrue(result is ViewResult);
        }

        [Test]
        public async Task Index_PostAddsPersonAndReturnsRedirect()
        {
            var firstName = "tim";
            var surname = "test";
            var dateOfBirth = new DateTime(2000,1,1);
            var emailAddress = "test@test.com";

            var result = await _controller.Index(new PersonViewModel { FirstName = firstName, Surname = surname, DateOfBirth = dateOfBirth, EmailAddress = emailAddress });

            await _partyRespository.Received().AddPerson(Arg.Is<Person>(o =>
                o.FirstName == firstName &&
                o.Surname == surname &&
                o.DateOfBirth == dateOfBirth &&
                o.EmailAddress == emailAddress));

            Assert.IsTrue(result is RedirectToActionResult);
        }

        [Test]
        public async Task Index_PostWithInvalidModelReturnsView()
        {
            var model = new PersonViewModel();
            _controller.ModelState.AddModelError("", "invalid model");

            var result = await _controller.Index(model);

            Assert.IsTrue(result is ViewResult);
            Assert.AreSame(model, (result as ViewResult).Model);
        }
    }
}
