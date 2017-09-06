using System.Linq;
using NUnit.Framework;
using PartyData.Entities;
using PartyData.Repositories;
using System.Threading.Tasks;

namespace PartyData.UnitTests.Respositories
{
    // These tests use an in memory DB to validate the complex linq queries work with simulated data
    [TestFixture]
    public class PartyRespositoryTests
    {
        private PartyDbContext _context;
        private PartyRespository _partyRespository;

        [SetUp]
        public void Setup()
        {
            _context = new PartyDbContext(TestHelper.GetInMemoryDbContextOptions());
            _context.Database.EnsureCreated();
            _partyRespository = new PartyRespository(_context);
        }

        [Test]
        public async Task GetCustomServices_ReturnsAll()
        {
            await AddCustomService(1, "Service1");
            await AddCustomService(2, "Service2");

            var result = await _partyRespository.GetCustomServices();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Service1", result[0].Name);
        }

        [Test]
        public async Task GetParty_Organisation()
        {
            await AddOrganisation(1, "Org1");

            var party = await _partyRespository.GetParty(1);

            Assert.AreEqual(1, party.PartyId);
            Assert.AreEqual("Org1", party.Organisation.OrganisationName);
        }

        [Test]
        public async Task GetParty_Person()
        {
            await AddPerson(1, "Person1");

            var party = await _partyRespository.GetParty(1);

            Assert.AreEqual(1, party.PartyId);
            Assert.AreEqual("Person1", party.Person.FirstName);
        }

        [Test]
        public async Task AddOrganisation_Adds()
        {
            await _partyRespository.AddOrganisation(new Organisation { OrganisationName = "Org1", TradingName = "Org1T" });

            var parties = _context.Parties.ToList();
            var organisations = _context.Organisations.ToList();
            Assert.AreEqual(1, parties.Count);
            Assert.AreEqual("Org1", parties[0].Name);
            Assert.AreEqual(1, organisations.Count);
            Assert.AreEqual("Org1", organisations[0].OrganisationName);
        }

        [Test]
        public async Task UpdateOrganisation_Updates()
        {
            await AddOrganisation(1, "Org1");

            await _partyRespository.UpdateOrganisation(new Organisation { PartyId = 1, OrganisationName = "Org1U", TradingName = "Org1T" });

            var parties = _context.Parties.ToList();
            var organisations = _context.Organisations.ToList();
            Assert.AreEqual("Org1U", parties[0].Name);
            Assert.AreEqual("Org1U", organisations[0].OrganisationName);
        }

        [Test]
        public async Task AddPerson_Adds()
        {
            await _partyRespository.AddPerson(new Person { FirstName = "Person1", Surname = "Org1T", DateOfBirth = new System.DateTime(), EmailAddress = "EmailAddress" });

            var parties = _context.Parties.ToList();
            var persons = _context.Persons.ToList();
            Assert.AreEqual(1, parties.Count);
            Assert.AreEqual("Person1 Org1T", parties[0].Name);
            Assert.AreEqual(1, persons.Count);
            Assert.AreEqual("Person1", persons[0].FirstName);
        }

        [Test]
        public async Task UpdatePerson_Updates()
        {
            await AddPerson(1, "Person1");

            await _partyRespository.UpdatePerson(new Person { PartyId = 1, FirstName = "Person1U", Surname = "Org1T", DateOfBirth = new System.DateTime(), EmailAddress = "EmailAddress" });

            var parties = _context.Parties.ToList();
            var persons = _context.Persons.ToList();
            Assert.AreEqual("Person1U Org1T", parties[0].Name);
            Assert.AreEqual("Person1U", persons[0].FirstName);
        }

        [Test]
        public async Task RegisterPartyWithService_RegistersParty()
        {
            await AddOrganisation(4321, "Org1");
            await AddCustomService(1234, "Service1");

            await _partyRespository.RegisterPartyWithService(4321, 1234);

            var registrations = _context.PartyCustomServiceRegistrations.ToList();
            Assert.AreEqual(1, registrations.Count);
            Assert.AreEqual(4321, registrations[0].PartyId);
            Assert.AreEqual(1234, registrations[0].CustomServiceId);
            Assert.IsTrue(registrations[0].RegistrationStatus);
        }

        [Test]
        public async Task RemovePartyFromCustomService_RemovesParty()
        {
            await AddOrganisation(4321, "Org1");
            await AddCustomService(1234, "Service1");
            await AddPartyCustomServiceRegistration(4321, 1234, true);

            await _partyRespository.RemovePartyFromCustomService(4321, 1234);

            var registrations = _context.PartyCustomServiceRegistrations.ToList();
            Assert.AreEqual(1, registrations.Count);
            Assert.AreEqual(4321, registrations[0].PartyId);
            Assert.AreEqual(1234, registrations[0].CustomServiceId);
            Assert.IsFalse(registrations[0].RegistrationStatus);
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

        private async Task AddPerson(int partyId, string name)
        {
            _context.Parties.Add(new Party { PartyId = partyId, Name = name });
            _context.Persons.Add(new Person { PartyId = partyId, PersonId = partyId, FirstName = name, Surname = "Surname", DateOfBirth = new System.DateTime(), EmailAddress = "EmailAddress" });
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
