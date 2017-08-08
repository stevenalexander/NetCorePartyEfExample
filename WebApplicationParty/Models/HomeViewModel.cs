using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PartyData.Entities;

namespace WebApplicationParty.Models
{
    public class HomeViewModel
    {
        public List<Party> Parties { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
