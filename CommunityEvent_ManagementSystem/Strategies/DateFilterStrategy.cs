using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Interfaces;

namespace CommunityEvent_ManagementSystem.Strategies
{
    public class DateFilterStrategy : IEventFilterStrategy
    {
        private readonly DateTime _date;

        public DateFilterStrategy(DateTime date)
        {
            _date = date;
        }

        public List<CommunityEvent> Filter(List<CommunityEvent> events)
        {
            return events
                .Where(e => e.EventDate.Date == _date.Date)
                .ToList();
        }
    }
}