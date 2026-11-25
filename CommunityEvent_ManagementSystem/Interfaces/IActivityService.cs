using CommunityEvent_ManagementSystem.Data.Models;

namespace CommunityEvent_ManagementSystem.Interfaces
{
    public interface IActivityService
    {
        Task<List<Activity>> GetAllAsync();
        Task<Activity?> GetByIdAsync(int id);
        Task AddAsync(Activity activity);
        Task UpdateAsync(Activity activity);
        Task DeleteAsync(int id);
    }
}