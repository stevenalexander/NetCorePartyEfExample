using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationParty.Models
{
    public class PersonViewModel
    {
        public int PartyId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string EmailAddress { get; set; }
    }
}
