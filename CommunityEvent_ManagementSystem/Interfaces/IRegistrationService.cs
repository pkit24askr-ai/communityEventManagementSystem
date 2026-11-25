using CommunityEvent_ManagementSystem.Data.Models;

namespace CommunityEvent_ManagementSystem.Interfaces
{
    public interface IRegistrationService
    {
        Task<List<Registration>> GetAllAsync();
        Task RegisterAsync(int participantId, int eventId);
        Task DeleteAsync(int id);
        Task UpdateAsync(Registration registration);
    }
}