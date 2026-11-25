using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Interfaces;

namespace CommunityEvent_ManagementSystem.Strategies
{
    public class EventFilterContext
    {
        private IEventFilterStrategy? _strategy;

        public void SetStrategy(IEventFilterStrategy strategy)
        {
            _strategy = strategy;
        }

        public List<CommunityEvent> ApplyFilter(List<CommunityEvent> events)
        {
            if (_strategy == null)
                return events;

            return _strategy.Filter(events);
        }
    }
}