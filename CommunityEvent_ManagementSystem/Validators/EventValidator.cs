using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Exceptions;

namespace CommunityEvent_ManagementSystem.Validators
{
    public static class EventValidator
    {
        public static void Validate(CommunityEvent eventItem)
        {
            if (string.IsNullOrWhiteSpace(eventItem.EventName))
                throw new InvalidEventException("Event name is required.");

            if (eventItem.EventName.Length < 3)
                throw new InvalidEventException("Event name must be at least 3 characters.");

            if (string.IsNullOrWhiteSpace(eventItem.Description))
                throw new InvalidEventException("Description is required.");

            if (eventItem.Description.Length < 10)
                throw new InvalidEventException("Description must be at least 10 characters.");

            if (eventItem.EventDate.Date < DateTime.Today)
                throw new InvalidEventException("Event date cannot be in the past.");

            if (eventItem.EndTime <= eventItem.StartTime)
                throw new InvalidEventException("End time must be after start time.");

            if (eventItem.Capacity <= 0)
                throw new InvalidEventException("Capacity must be greater than 0.");
        }
    }
}