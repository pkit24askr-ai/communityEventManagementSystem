using System.ComponentModel.DataAnnotations;

namespace CommunityEvent_ManagementSystem.Data.Models
{
    public class Participant
    {
        public int Id { get; set; }

        [Required, StringLength(50, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}