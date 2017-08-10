using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartyData.Entities;
using Microsoft.EntityFrameworkCore;

namespace PartyData.Repositories
{
    // Should be split into multiple respositories, e.g. Person, Organisation
    public class PartyRespository : IPartyRespository
    {
        private PartyDbContext _partyDbContext;

        public PartyRespository(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        public async Task<List<Party>> GetPartiesWithRegistrations()
        {
            // multiple includes returning all results bad for large volumes of data
            return await _partyDbContext.Parties
                .Include(p => p.CustomServiceRegistrations)
                .Include(p => p.Organisation)
                .Include(p => p.Person)
                .ToListAsync();
        }

        public async Task<List<CustomService>> GetCustomServices()
        {
            return await _partyDbContext.CustomServices.ToListAsync();
        }

        // Should be split into GetPerson and GetOrganisation
        public async Task<Party> GetParty(int partyId)
        {
            return await _partyDbContext.Parties
                .Include(p => p.Organisation)
                .Include(p => p.Person)
                .FirstOrDefaultAsync(p => p.PartyId == partyId);
        }

        public async Task AddOrganisation(Organisation organisation)
        {
            organisation.Party = new Party(organisation.GetPartyName());
            _partyDbContext.Add(organisation);
            await _partyDbContext.SaveChangesAsync();
        }

        public async Task UpdateOrganisation(Organisation organisation)
        {
            var party = await GetParty(organisation.PartyId);

            // This should be done as a mapper
            party.Organisation.OrganisationName = organisation.OrganisationName;
            party.Organisation.TradingName = organisation.TradingName;
            party.Name = organisation.GetPartyName();

            _partyDbContext.Update(party);
            _partyDbContext.Update(party.Organisation);
            await _partyDbContext.SaveChangesAsync();
        }

        public async Task AddPerson(Person person)
        {
            person.Party = new Party(person.GetPartyName());
            _partyDbContext.Add(person);
            await _partyDbContext.SaveChangesAsync();
        }

        public async Task UpdatePerson(Person person)
        {
            var party = await GetParty(person.PartyId);

            // This should be done as a mapper
            party.Person.FirstName = person.FirstName;
            party.Person.Surname = person.Surname;
            party.Person.DateOfBirth = person.DateOfBirth;
            party.Person.EmailAddress = person.EmailAddress;
            party.Name = person.GetPartyName();

            _partyDbContext.Update(party);
            _partyDbContext.Update(party.Person);
            await _partyDbContext.SaveChangesAsync();
        }

        public async Task RegisterPartyWithService(int partyId, int customServiceId)
        {
            if (customServiceId > 0 && partyId > 0)
            {
                var existingRegistration = await _partyDbContext.PartyCustomServiceRegistrations.FirstOrDefaultAsync(r => r.PartyId == partyId && r.CustomServiceId == customServiceId);

                if (existingRegistration == null)
                {
                    var newRegistration = new PartyCustomServiceRegistration
                    {
                        PartyId = partyId,
                        CustomServiceId = customServiceId,
                        RegistrationStatus = true
                    };

                    _partyDbContext.Add(newRegistration);
                    await _partyDbContext.SaveChangesAsync();
                }
                else if (!existingRegistration.RegistrationStatus)
                {
                    existingRegistration.RegistrationStatus = true;
                    _partyDbContext.Update(existingRegistration);
                    await _partyDbContext.SaveChangesAsync();
                }
            }
        }

        public async Task RemovePartyFromCustomService(int partyId, int customServiceId)
        {
            if (customServiceId > 0 && partyId > 0)
            {
                var registration = _partyDbContext.PartyCustomServiceRegistrations.FirstOrDefault(r =>
                    r.PartyId == partyId &&
                    r.CustomServiceId == customServiceId &&
                    r.RegistrationStatus);

                if (registration != null)
                {
                    registration.RegistrationStatus = false;
                    _partyDbContext.Update(registration);
                    await _partyDbContext.SaveChangesAsync();
                }
            }
        }
    }
}
