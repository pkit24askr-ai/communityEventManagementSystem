using CommunityEvent_ManagementSystem.Data;
using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvent_ManagementSystem.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;

        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Activity>> GetAllAsync()
        {
            return await _context.Activities.OrderBy(a => a.ActivityName).ToListAsync();
        }

        public async Task<Activity?> GetByIdAsync(int id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task AddAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity activity)
        {
            var existingActivity = await _context.Activities
                .FirstOrDefaultAsync(a => a.Id == activity.Id);

            if (existingActivity == null)
                throw new InvalidOperationException("Activity not found.");

            existingActivity.ActivityName = activity.ActivityName;
            existingActivity.ActivityType = activity.ActivityType;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities
                .Include(a => a.EventActivities)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activity == null)
                throw new InvalidOperationException("Activity not found.");

            if (activity.EventActivities.Any())
                throw new InvalidOperationException(
                    "Cannot delete this activity because it is assigned to one or more events.");

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
        }
    }
}