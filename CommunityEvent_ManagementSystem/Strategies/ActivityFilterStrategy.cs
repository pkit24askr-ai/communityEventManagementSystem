using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Interfaces;

namespace CommunityEvent_ManagementSystem.Strategies
{
    public class ActivityFilterStrategy : IEventFilterStrategy
    {
        private readonly int _activityId;

        public ActivityFilterStrategy(int activityId)
        {
            _activityId = activityId;
        }

        public List<CommunityEvent> Filter(List<CommunityEvent> events)
        {
            return events
                .Where(e => e.EventActivities.Any(a => a.ActivityId == _activityId))
                .ToList();
        }
    }
}