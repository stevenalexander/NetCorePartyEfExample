using System.ComponentModel.DataAnnotations;

namespace PartyData.Entities
{
    public class PartyCustomServiceRegistration
    {
        [Key]
        public int PartyCustomServiceRegistrationId { get; set; }

        [Required]
        public int PartyId { get; set; }

        [Required]
        public int CustomServiceId { get; set; }

        [Required]
        public bool RegistrationStatus { get; set; }

        public Party Party { get; set; }

        public CustomService CustomService { get; set; }
    }
}
