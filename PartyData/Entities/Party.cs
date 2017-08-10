using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        [NotMapped]
        public List<PartyCustomServiceRegistration> ActiveRegistrations
        {
            get
            {
                return CustomServiceRegistrations.Where(r => r.RegistrationStatus).ToList();
            }
        }

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
