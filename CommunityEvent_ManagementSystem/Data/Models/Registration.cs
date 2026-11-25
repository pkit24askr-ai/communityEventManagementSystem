using System.ComponentModel.DataAnnotations;

namespace CommunityEvent_ManagementSystem.Data.Models
{
    public class Registration
    {
        public int Id { get; set; }

        [Required]
        public int ParticipantId { get; set; }
        public Participant? Participant { get; set; }

        [Required]
        public int CommunityEventId { get; set; }
        public CommunityEvent? CommunityEvent { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Registered";
    }
}