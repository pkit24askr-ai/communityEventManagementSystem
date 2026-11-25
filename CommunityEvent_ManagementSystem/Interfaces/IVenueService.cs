using CommunityEvent_ManagementSystem.Data.Models;

namespace CommunityEvent_ManagementSystem.Interfaces
{
    public interface IVenueService
    {
        Task<List<Venue>> GetAllAsync();
        Task<Venue?> GetByIdAsync(int id);
        Task AddAsync(Venue venue);
        Task UpdateAsync(Venue venue);
        Task DeleteAsync(int id);
    }
}