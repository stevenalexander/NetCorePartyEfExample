using System.Collections.Generic;
using PartyData.Entities;

namespace WebApplicationParty.Models
{
    public class HomeViewModel
    {
        public List<Party> Parties { get; set; }

        public List<CustomService> Services { get; set; }

        public int PartyId { get; set; }

        public int CustomServiceId { get; set; }
    }
}
