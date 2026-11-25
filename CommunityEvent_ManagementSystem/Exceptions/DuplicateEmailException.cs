namespace CommunityEvent_ManagementSystem.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException(string message) : base(message)
        {
        }
    }
}