using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Services;
using CommunityEvent_ManagementSystem.Tests.Helpers;
using Xunit;

namespace CommunityEvent_ManagementSystem.Tests
{
    public class ActivityServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddActivity()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new ActivityService(context);

            var activity = new Activity
            {
                ActivityName = "Workshop",
                ActivityType = "Education"
            };

            await service.AddAsync(activity);

            var activities = await service.GetAllAsync();

            Assert.Single(activities);
            Assert.Equal("Workshop", activities[0].ActivityName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateActivity()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new ActivityService(context);

            var activity = new Activity
            {
                ActivityName = "Old Activity",
                ActivityType = "Old Type"
            };

            await service.AddAsync(activity);

            activity.ActivityName = "Updated Activity";
            await service.UpdateAsync(activity);

            var updated = await service.GetByIdAsync(activity.Id);

            Assert.Equal("Updated Activity", updated!.ActivityName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenActivityAssignedToEvent()
        {
            await using var context = TestDbContext.CreateContext();

            context.Activities.Add(new Activity
            {
                Id = 1,
                ActivityName = "Used Activity",
                ActivityType = "Education"
            });

            context.EventActivities.Add(new EventActivity
            {
                Id = 1,
                ActivityId = 1,
                CommunityEventId = 1
            });

            await context.SaveChangesAsync();

            var service = new ActivityService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(1));
        }
    }
}