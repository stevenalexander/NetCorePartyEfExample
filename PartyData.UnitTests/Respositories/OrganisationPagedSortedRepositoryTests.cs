using System.Linq;
using NUnit.Framework;
using PartyData.Entities;
using PartyData.Repositories;
using System.Threading.Tasks;
using System;

namespace PartyData.UnitTests.Respositories
{
    // These tests use an in memory DB to validate the complex linq queries work with simulated data
    [TestFixture]
    public class OrganisationPagedSortedRepositoryTests
    {
        private PartyDbContext _context;
        private OrganisationPagedSortedRepository _respository;

        [SetUp]
        public void Setup()
        {
            _context = new PartyDbContext(TestHelper.GetInMemoryDbContextOptions());
            _context.Database.EnsureCreated();
            _respository = new OrganisationPagedSortedRepository(_context);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResults()
        {
            var length = 10;
            await AddOrganisations(11);

            var result = await _respository.GetPagedSortedResults(0, length, "Name", true);

            Assert.AreEqual(11, result.recordsTotal);
            Assert.AreEqual(11, result.recordsFiltered);
            Assert.AreEqual(10, result.data.Count());
            Assert.AreEqual("Test1", result.data.First().Name);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsPaged()
        {
            var length = 5;
            await AddOrganisations(9);

            var result = await _respository.GetPagedSortedResults(5, length, "Name", true);

            Assert.AreEqual(9, result.recordsTotal);
            Assert.AreEqual(4, result.data.Count());
            Assert.AreEqual("Test6", result.data.First().Name);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsSortedByName()
        {
            var length = 10;
            await AddOrganisations(9);

            var result = await _respository.GetPagedSortedResults(0, length, "Name", false);

            Assert.AreEqual("Test9", result.data.First().Name);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsSortedByDateCreated()
        {
            var length = 10;
            await AddOrganisations(9);

            var result = await _respository.GetPagedSortedResults(0, length, "DateCreated", true);

            Assert.AreEqual(new DateTime(2000 - 9, 1, 1), result.data.First().DateCreated);
        }

        private async Task AddOrganisations(int count)
        {
            for (var i = 1; i <= count; i++)
            {
                await AddOrganisation(i, $"Test{i}", new DateTime(2000 - i, 1, 1));
            }
        }

        private async Task AddOrganisation(int partyId, string name, DateTime dateCreated)
        {
            _context.Parties.Add(new Party { PartyId = partyId, Name = name, DateCreated = dateCreated });
            _context.Organisations.Add(new Organisation { PartyId = partyId, OrganisationId = partyId, OrganisationName = name, TradingName = name });
            await _context.SaveChangesAsync();
        }
    }
}
