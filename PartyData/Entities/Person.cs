using System;
using System.ComponentModel.DataAnnotations;

namespace PartyData.Entities
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        [Required]
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

        public Party Party { get; set; }
    }
}
