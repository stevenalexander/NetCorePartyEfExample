using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationParty.Models
{
    public class PersonViewModel : HomeViewModel
    {
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
