using System;
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
    }
}
