using System.Collections.Generic;

namespace PartyData.Data
{
    public class PartyWithRegistrationsResultItem
    {
        public int PartyId { get; set; }

        public string Name { get; set; }

        public IEnumerable<int> ActiveRegistrationCustomServiceIds { get; set; }
    }
}