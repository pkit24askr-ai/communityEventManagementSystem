using CommunityEvent_ManagementSystem.Data.Models;

namespace CommunityEvent_ManagementSystem.Interfaces
{
    public interface IParticipantService
    {
        Task<List<Participant>> GetAllAsync();
        Task<Participant?> GetByIdAsync(int id);
        Task AddAsync(Participant participant);
        Task UpdateAsync(Participant participant);
        Task DeleteAsync(int id);
    }
}