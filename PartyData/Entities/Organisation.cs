using System.ComponentModel.DataAnnotations;

namespace PartyData.Entities
{
    public class Organisation
    {
        [Key]
        public int OrganisationId { get; set; }

        [Required]
        public int PartyId { get; set; }

        [Required]
        public string OrganisationName { get; set; }

        [Required]
        public string TradingName { get; set; }

        public Party Party { get; set; }
    }
}
