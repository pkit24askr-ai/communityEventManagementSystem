using CommunityEvent_ManagementSystem.Data.Models;

namespace CommunityEvent_ManagementSystem.Validators
{
    public static class ParticipantValidator
    {
        public static void Validate(Participant participant)
        {
            if (string.IsNullOrWhiteSpace(participant.FullName))
                throw new ArgumentException("Full name is required.");

            if (participant.FullName.Length < 2)
                throw new ArgumentException("Full name must be at least 2 characters.");

            if (string.IsNullOrWhiteSpace(participant.Email))
                throw new ArgumentException("Email is required.");

            if (!participant.Email.Contains("@"))
                throw new ArgumentException("Please enter a valid email address.");

            if (string.IsNullOrWhiteSpace(participant.PhoneNumber))
                throw new ArgumentException("Phone number is required.");

            if (participant.PhoneNumber.Length < 7)
                throw new ArgumentException("Phone number must be valid.");
        }
    }
}