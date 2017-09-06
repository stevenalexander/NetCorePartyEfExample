using System.Collections.Generic;
using PartyData.Entities;
using PartyData.Data;

namespace WebApplicationParty.Models
{
    public class HomeViewModel : PagedSortedViewModel<PartyWithRegistrationsResultItem>
    {
        public List<CustomService> Services { get; set; }

        public int PartyId { get; set; }

        public int CustomServiceId { get; set; }
    }
}
