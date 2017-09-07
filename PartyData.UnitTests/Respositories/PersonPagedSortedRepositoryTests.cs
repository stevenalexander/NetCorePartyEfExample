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
    public class PersonPagedSortedRepositoryTests
    {
        private PartyDbContext _context;
        private PersonPagedSortedRepository _respository;

        [SetUp]
        public void Setup()
        {
            _context = new PartyDbContext(TestHelper.GetInMemoryDbContextOptions());
            _context.Database.EnsureCreated();
            _respository = new PersonPagedSortedRepository(_context);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResults()
        {
            var length = 10;
            await AddPersons(11);

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
            await AddPersons(9);

            var result = await _respository.GetPagedSortedResults(5, length, "Name", true);

            Assert.AreEqual(9, result.recordsTotal);
            Assert.AreEqual(4, result.data.Count());
            Assert.AreEqual("Test6", result.data.First().Name);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsSortedByName()
        {
            var length = 10;
            await AddPersons(9);

            var result = await _respository.GetPagedSortedResults(0, length, "Name", false);

            Assert.AreEqual("Test9", result.data.First().Name);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsSortedByEmailAddress()
        {
            var length = 10;
            await AddPersons(9);

            var result = await _respository.GetPagedSortedResults(0, length, "EmailAddress", false);

            Assert.AreEqual("Test9@test.com", result.data.First().EmailAddress);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsSortedByDateOfBirth()
        {
            var length = 10;
            await AddPersons(9);

            var result = await _respository.GetPagedSortedResults(0, length, "DateOfBirth", true);

            Assert.AreEqual(new DateTime(1980 - 9, 1, 1), result.data.First().DateOfBirth);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsSortedByDateCreated()
        {
            var length = 10;
            await AddPersons(9);

            var result = await _respository.GetPagedSortedResults(0, length, "DateCreated", true);

            Assert.AreEqual(new DateTime(1980 - 9, 1, 1), result.data.First().DateCreated);
        }

        private async Task AddPersons(int count)
        {
            for (var i = 1; i <= count; i++)
            {
                await AddPerson(i, $"Test{i}", new DateTime(1980 - i, 1, 1));
            }
        }

        private async Task AddPerson(int partyId, string name, DateTime dateCreated)
        {
            _context.Parties.Add(new Party { PartyId = partyId, Name = name, DateCreated = dateCreated });
            _context.Persons.Add(new Person { PartyId = partyId, PersonId = partyId, FirstName = $"{name}f", Surname = $"{name}s", EmailAddress = $"{name}@test.com", DateOfBirth = dateCreated });
            await _context.SaveChangesAsync();
        }
    }
}
