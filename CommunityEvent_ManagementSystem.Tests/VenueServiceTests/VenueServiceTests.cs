using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Services;
using CommunityEvent_ManagementSystem.Tests.Helpers;
using Xunit;

namespace CommunityEvent_ManagementSystem.Tests
{
    public class VenueServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddVenue()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new VenueService(context);

            var venue = new Venue
            {
                VenueName = "Main Hall",
                Address = "City Centre",
                Capacity = 100
            };

            await service.AddAsync(venue);

            Assert.True(venue.Id > 0);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateVenue()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new VenueService(context);

            var venue = new Venue
            {
                VenueName = "Old Hall",
                Address = "Old Location",
                Capacity = 50
            };

            await service.AddAsync(venue);

            venue.VenueName = "Updated Hall";
            await service.UpdateAsync(venue);

            var updated = await service.GetByIdAsync(venue.Id);

            Assert.Equal("Updated Hall", updated!.VenueName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenVenueAssignedToEvent()
        {
            await using var context = TestDbContext.CreateContext();

            context.Venues.Add(new Venue
            {
                Id = 1,
                VenueName = "Used Venue",
                Address = "City",
                Capacity = 100
            });

            context.EventVenues.Add(new EventVenue
            {
                Id = 1,
                VenueId = 1,
                CommunityEventId = 1
            });

            await context.SaveChangesAsync();

            var service = new VenueService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(1));
        }
    }
}