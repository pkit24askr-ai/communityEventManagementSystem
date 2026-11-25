using CommunityEvent_ManagementSystem.Data.Models;

namespace CommunityEvent_ManagementSystem.Factories
{
    public static class EventFactory
    {
        public static CommunityEvent CreateEvent(
            string eventName,
            string description,
            DateTime eventDate,
            TimeSpan startTime,
            TimeSpan endTime,
            int capacity)
        {
            return new CommunityEvent
            {
                EventName = eventName,
                Description = description,
                EventDate = eventDate,
                StartTime = startTime,
                EndTime = endTime,
                Capacity = capacity
            };
        }
    }
}