namespace CommunityEvent_ManagementSystem.Exceptions
{
    public class EventFullException : Exception
    {
        public EventFullException(string message) : base(message)
        {
        }
    }
}