using System.Linq;
using NUnit.Framework;
using PartyData.Entities;
using PartyData.Repositories;
using System.Threading.Tasks;

namespace PartyData.UnitTests.Respositories
{
    // These tests use an in memory DB to validate the complex linq queries work with simulated data
    [TestFixture]
    public class PartyWithRegistrationsPagedSortedRepositoryTests
    {
        private PartyDbContext _context;
        private PartyWithRegistrationsPagedSortedRepository _respository;

        [SetUp]
        public void Setup()
        {
            _context = new PartyDbContext(TestHelper.GetInMemoryDbContextOptions());
            _context.Database.EnsureCreated();
            _respository = new PartyWithRegistrationsPagedSortedRepository(_context);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResults()
        {
            var length = 10;
            await AddPartiesWithRegistrations(11);

            var result = await _respository.GetPagedSortedResults(0, length, "Name", true);

            Assert.AreEqual(11, result.recordsTotal);
            Assert.AreEqual(11, result.recordsFiltered);
            Assert.AreEqual(10, result.data.Count());
            Assert.AreEqual("Test1", result.data.First().Name);
            Assert.AreEqual(1, result.data.First().ActiveRegistrationCustomServiceIds.Count());
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsPaged()
        {
            var length = 5;
            await AddPartiesWithRegistrations(9);

            var result = await _respository.GetPagedSortedResults(5, length, "Name", true);

            Assert.AreEqual(9, result.recordsTotal);
            Assert.AreEqual(4, result.data.Count());
            Assert.AreEqual("Test6", result.data.First().Name);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsSorted()
        {
            var length = 10;
            await AddPartiesWithRegistrations(9);

            var result = await _respository.GetPagedSortedResults(0, length, "Name", false);

            Assert.AreEqual("Test9", result.data.First().Name);
        }

        [Test]
        public async Task GetPagedSortedResults_ReturnsPagedSortedResultsWithActiveRegistrations()
        {
            var length = 5;
            await AddPartiesWithRegistrations(5);
            await AddCustomService(2, "Service2");
            await AddCustomService(3, "Service3");
            await AddPartyCustomServiceRegistration(1, 2, false);
            await AddPartyCustomServiceRegistration(1, 3, true);

            var result = await _respository.GetPagedSortedResults(0, length, "Name", true);

            Assert.AreEqual(2, result.data.First().ActiveRegistrationCustomServiceIds.Count());
            Assert.AreEqual(1, result.data.First().ActiveRegistrationCustomServiceIds.ToList()[0]);
            Assert.AreEqual(3, result.data.First().ActiveRegistrationCustomServiceIds.ToList()[1]);
        }

        private async Task AddPartiesWithRegistrations(int count)
        {
            await AddCustomService(1, "Service1");

            for (var i = 1; i <= count; i++)
            {
                await AddOrganisation(i, $"Test{i}");
                await AddPartyCustomServiceRegistration(i, 1, true);
            }
        }

        private async Task AddCustomService(int customServiceId, string name)
        {
            _context.CustomServices.Add(new CustomService { CustomServiceId = customServiceId, Name = name });
            await _context.SaveChangesAsync();
        }

        private async Task AddOrganisation(int partyId, string name)
        {
            _context.Parties.Add(new Party { PartyId = partyId, Name = name });
            _context.Organisations.Add(new Organisation { PartyId = partyId, OrganisationId = partyId, OrganisationName = name, TradingName = name });
            await _context.SaveChangesAsync();
        }

        private async Task AddPartyCustomServiceRegistration(int partyId, int customServiceId, bool registrationStatus)
        {
            _context.PartyCustomServiceRegistrations.Add(new PartyCustomServiceRegistration
            {
                PartyId = partyId,
                CustomServiceId = customServiceId,
                RegistrationStatus = registrationStatus
            });
            await _context.SaveChangesAsync();
        }
    }
}
