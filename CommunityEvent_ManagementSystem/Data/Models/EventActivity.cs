namespace CommunityEvent_ManagementSystem.Data.Models
{
    public class EventActivity
    {
        public int Id { get; set; }

        public int CommunityEventId { get; set; }
        public CommunityEvent? CommunityEvent { get; set; }

        public int ActivityId { get; set; }
        public Activity? Activity { get; set; }
    }
}