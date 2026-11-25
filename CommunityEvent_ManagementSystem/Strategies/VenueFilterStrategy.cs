using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Interfaces;

namespace CommunityEvent_ManagementSystem.Strategies
{
    public class VenueFilterStrategy : IEventFilterStrategy
    {
        private readonly int _venueId;

        public VenueFilterStrategy(int venueId)
        {
            _venueId = venueId;
        }

        public List<CommunityEvent> Filter(List<CommunityEvent> events)
        {
            return events
                .Where(e => e.EventVenues.Any(v => v.VenueId == _venueId))
                .ToList();
        }
    }
}