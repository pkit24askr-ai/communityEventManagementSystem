using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CommunityEvent_ManagementSystem.Data.Models
{
    public class CommunityEvent
    {
        public int Id { get; set; }

        [Required, StringLength(100, MinimumLength = 3)]
        public string EventName { get; set; } = string.Empty;

        [Required, StringLength(500, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; } = DateTime.Today;

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Range(1, 1000)]
        public int Capacity { get; set; }

        [NotMapped]
        public List<int> SelectedVenueIds { get; set; } = new();

        [NotMapped]
        public List<int> SelectedActivityIds { get; set; } = new();

        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
        public ICollection<EventVenue> EventVenues { get; set; } = new List<EventVenue>();
        public ICollection<EventActivity> EventActivities { get; set; } = new List<EventActivity>();
    }
}