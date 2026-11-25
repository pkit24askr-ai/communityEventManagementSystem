using System.ComponentModel.DataAnnotations;

namespace CommunityEvent_ManagementSystem.Data.Models
{
    public class Venue
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string VenueName { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Range(1, 1000)]
        public int Capacity { get; set; }

        public ICollection<EventVenue> EventVenues { get; set; } = new List<EventVenue>();
    }
}