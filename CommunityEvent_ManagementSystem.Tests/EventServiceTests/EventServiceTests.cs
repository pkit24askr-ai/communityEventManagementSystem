using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Exceptions;
using CommunityEvent_ManagementSystem.Services;
using CommunityEvent_ManagementSystem.Tests.Helpers;
using Xunit;

namespace CommunityEvent_ManagementSystem.Tests
{
    public class EventServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddValidEvent()
        {
            await using var context = TestDbContext.CreateContext();

            context.Venues.Add(new Venue
            {
                Id = 1,
                VenueName = "Main Hall",
                Address = "City",
                Capacity = 100
            });
            context.Activities.Add(new Activity { Id = 1, ActivityName = "Workshop", ActivityType = "Education" });
            await context.SaveChangesAsync();

            var service = new EventService(context);

            var eventItem = new CommunityEvent
            {
                EventName = "Community Workshop",
                Description = "A valid test event",
                EventDate = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Capacity = 50,
                SelectedVenueIds = new List<int> { 1 },
                SelectedActivityIds = new List<int> { 1 }
            };

            await service.AddAsync(eventItem);

            var events = await service.GetAllAsync();

            Assert.Single(events);
            Assert.Equal("Community Workshop", events[0].EventName);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenEventDateIsInPast()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new EventService(context);

            var eventItem = new CommunityEvent
            {
                EventName = "Old Event",
                Description = "Invalid old event",
                EventDate = DateTime.Today.AddDays(-1),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Capacity = 20,
                SelectedVenueIds = new List<int> { 1 },
                SelectedActivityIds = new List<int> { 1 }
            };

            await Assert.ThrowsAsync<InvalidEventException>(() => service.AddAsync(eventItem));
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenEndTimeBeforeStartTime()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new EventService(context);

            var eventItem = new CommunityEvent
            {
                EventName = "Invalid Time Event",
                Description = "End time is before start time",
                EventDate = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(12, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Capacity = 20,
                SelectedVenueIds = new List<int> { 1 },
                SelectedActivityIds = new List<int> { 1 }
            };

            await Assert.ThrowsAsync<InvalidEventException>(() => service.AddAsync(eventItem));
        }


        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenEventHasRegistrations()
        {
            await using var context = TestDbContext.CreateContext();

            context.CommunityEvents.Add(new CommunityEvent
            {
                Id = 1,
                EventName = "Registered Event",
                Description = "Event with registration",
                EventDate = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Capacity = 20
            });

            context.Registrations.Add(new Registration
            {
                Id = 1,
                CommunityEventId = 1,
                ParticipantId = 1,
                RegistrationDate = DateTime.Now,
                Status = "Registered"
            });

            await context.SaveChangesAsync();

            var service = new EventService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(1));
        }
    }
}