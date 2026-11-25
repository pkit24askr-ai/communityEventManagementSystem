using CommunityEvent_ManagementSystem.Data.Models;

namespace CommunityEvent_ManagementSystem.Interfaces
{
    public interface IEventService
    {
        Task<List<CommunityEvent>> GetAllAsync();
        Task<CommunityEvent?> GetByIdAsync(int id);
        Task AddAsync(CommunityEvent eventItem);
        Task UpdateAsync(CommunityEvent eventItem);
        Task DeleteAsync(int id);


    }
}