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
                var existingRegistrations = await _partyDbContext.PartyCustomServiceRegistrations.Where(r => r.PartyId == partyId).ToListAsync();

                if (!existingRegistrations.Exists(r => r.CustomServiceId == customServiceId))
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
            }
        }
    }
}
