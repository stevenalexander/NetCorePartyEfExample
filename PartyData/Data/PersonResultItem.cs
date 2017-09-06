using System;

namespace PartyData.Data
{
    public class PersonResultItem
    {
        public int PartyId { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime DateCreated { get; set; }
    }
}