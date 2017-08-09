using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PartyData.Entities
{
    public class CustomService
    {
        [Key]
        public int CustomServiceId { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Party> RegisteredParties { get; set; }
    }
}
