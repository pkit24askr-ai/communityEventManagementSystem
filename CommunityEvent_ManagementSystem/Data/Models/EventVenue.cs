namespace CommunityEvent_ManagementSystem.Data.Models
{
    public class EventVenue
    {
        public int Id { get; set; }

        public int CommunityEventId { get; set; }
        public CommunityEvent? CommunityEvent { get; set; }

        public int VenueId { get; set; }
        public Venue? Venue { get; set; }
    }
}