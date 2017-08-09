using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartyData.Entities
{
    public class Party
    {
        [Key]
        public int PartyId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateLastModified { get; set; }

        public List<PartyCustomServiceRegistration> CustomServiceRegistrations { get; set; }

        public Party() // required empty constructor
        {
        }

        public Party(string name)
        {
            Name = name;
            DateCreated = DateTime.UtcNow;
            DateLastModified = DateTime.UtcNow;
        }
    }
}
