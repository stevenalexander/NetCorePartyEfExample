using System.ComponentModel.DataAnnotations;

namespace WebApplicationParty.Models
{
    public class OrganisationViewModel
    {
        [Required]
        public string OrganisationName { get; set; }

        [Required]
        public string TradingName { get; set; }
    }
}
