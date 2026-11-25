namespace CommunityEvent_ManagementSystem.Exceptions
{
    public class DuplicateRegistrationException : Exception
    {
        public DuplicateRegistrationException(string message) : base(message)
        {
        }
    }
}