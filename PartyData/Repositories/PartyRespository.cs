using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PartyData.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace PartyData.Repositories
{
    public class PartyRespository : IPartyRespository
    {
        private PartyDbContext _partyDbContext;

        public PartyRespository(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        public async Task<List<Party>> GetPartiesWithRegistrations()
        {
            return await _partyDbContext.Parties.Include(p => p.CustomServiceRegistrations).ToListAsync();
        }

        public async Task<List<CustomService>> GetCustomServices()
        {
            return await _partyDbContext.CustomServices.ToListAsync();
        }

        public async Task AddOrganisation(Organisation organisation)
        {
            _partyDbContext.Add(organisation);
            await _partyDbContext.SaveChangesAsync();
        }

        public async Task AddPerson(Person person)
        {
            _partyDbContext.Add(person);
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
