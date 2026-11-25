using System.ComponentModel.DataAnnotations;

namespace CommunityEvent_ManagementSystem.Data.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string ActivityName { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string ActivityType { get; set; } = string.Empty;

        public ICollection<EventActivity> EventActivities { get; set; } = new List<EventActivity>();
    }
}