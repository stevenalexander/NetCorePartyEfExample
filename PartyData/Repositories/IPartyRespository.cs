using PartyData.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PartyData.Repositories
{
    public interface IPartyRespository
    {
        Task<List<Party>> GetPartiesWithRegistrations();

        Task<List<CustomService>> GetCustomServices();

        Task<Party> GetParty(int partyId);

        Task AddOrganisation(Organisation organisation);

        Task UpdateOrganisation(Organisation organisation);

        Task AddPerson(Person person);

        Task UpdatePerson(Person person);

        Task RegisterPartyWithService(int partyId, int customServiceId);

        Task RemovePartyFromCustomService(int partyId, int customServiceId);
    }
}
