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
    public class PersonPagedSortedTableControllerTests
    {
        private PersonPagedSortedTableController _controller;
        private IPagedSortedRepository<PersonResultItem> _pagedSortedRepository;

        [SetUp]
        public void SetUp()
        {
            _pagedSortedRepository = Substitute.For<IPagedSortedRepository<PersonResultItem>>();
            _controller = new PersonPagedSortedTableController(_pagedSortedRepository);
        }

        [Test]
        public async Task Index_GetsDataAndReturnsView()
        {
            var pagedSortedResult = new PagedSortedResult<PersonResultItem>()
            {
                recordsFiltered = 50,
                recordsTotal = 100,
                data = new List<PersonResultItem>(),
            };
            
            _pagedSortedRepository.GetPagedSortedResults(0, 10, "Name", true).Returns(pagedSortedResult);
            
            var result = await _controller.Index();

            Assert.IsTrue(result is ViewResult);
            Assert.AreEqual(pagedSortedResult.recordsTotal, ((result as ViewResult).Model as PagedSortedViewModel<PersonResultItem>).RecordsTotal);
            Assert.AreEqual(pagedSortedResult.recordsFiltered, ((result as ViewResult).Model as PagedSortedViewModel<PersonResultItem>).RecordsFiltered);
            Assert.AreSame(pagedSortedResult.data, ((result as ViewResult).Model as PagedSortedViewModel<PersonResultItem>).Data);
        }
    }
}
