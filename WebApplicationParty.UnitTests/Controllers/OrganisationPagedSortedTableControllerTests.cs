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
    public class OrganisationPagedSortedTableControllerTests
    {
        private OrganisationPagedSortedTableController _controller;
        private IPagedSortedRepository<OrganisationResultItem> _pagedSortedRepository;

        [SetUp]
        public void SetUp()
        {
            _pagedSortedRepository = Substitute.For<IPagedSortedRepository<OrganisationResultItem>>();
            _controller = new OrganisationPagedSortedTableController(_pagedSortedRepository);
        }

        [Test]
        public async Task Index_GetsDataAndReturnsView()
        {
            var pagedSortedResult = new PagedSortedResult<OrganisationResultItem>()
            {
                recordsFiltered = 50,
                recordsTotal = 100,
                data = new List<OrganisationResultItem>(),
            };
            
            _pagedSortedRepository.GetPagedSortedResults(0, 10, "Name", true).Returns(pagedSortedResult);
            
            var result = await _controller.Index();

            Assert.IsTrue(result is ViewResult);
            Assert.AreEqual(pagedSortedResult.recordsTotal, ((result as ViewResult).Model as PagedSortedViewModel<OrganisationResultItem>).RecordsTotal);
            Assert.AreEqual(pagedSortedResult.recordsFiltered, ((result as ViewResult).Model as PagedSortedViewModel<OrganisationResultItem>).RecordsFiltered);
            Assert.AreSame(pagedSortedResult.data, ((result as ViewResult).Model as PagedSortedViewModel<OrganisationResultItem>).Data);
        }
    }
}
