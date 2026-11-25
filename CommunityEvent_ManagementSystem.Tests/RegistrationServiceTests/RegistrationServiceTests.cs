using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Exceptions;
using CommunityEvent_ManagementSystem.Services;
using CommunityEvent_ManagementSystem.Tests.Helpers;
using Xunit;

namespace CommunityEvent_ManagementSystem.Tests
{
    public class RegistrationServiceTests
    {
        [Fact]
        public async Task RegisterAsync_ShouldRegisterParticipant()
        {
            await using var context = TestDbContext.CreateContext();

            context.Participants.Add(new Participant
            {
                Id = 1,
                FullName = "Test Participant",
                Email = "test@test.com",
                PhoneNumber = "9800000000"
            });

            context.CommunityEvents.Add(new CommunityEvent
            {
                Id = 1,
                EventName = "Test Event",
                Description = "Event description",
                EventDate = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Capacity = 5
            });

            await context.SaveChangesAsync();

            var service = new RegistrationService(context);

            await service.RegisterAsync(1, 1);

            var registrations = await service.GetAllAsync();

            Assert.Single(registrations);
            Assert.Equal("Registered", registrations[0].Status);
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenDuplicateRegistration()
        {
            await using var context = TestDbContext.CreateContext();

            context.Participants.Add(new Participant
            {
                Id = 1,
                FullName = "Test Participant",
                Email = "test@test.com",
                PhoneNumber = "9800000000"
            });

            context.CommunityEvents.Add(new CommunityEvent
            {
                Id = 1,
                EventName = "Test Event",
                Description = "Event description",
                EventDate = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Capacity = 5
            });

            context.Registrations.Add(new Registration
            {
                Id = 1,
                ParticipantId = 1,
                CommunityEventId = 1,
                RegistrationDate = DateTime.Now,
                Status = "Registered"
            });

            await context.SaveChangesAsync();

            var service = new RegistrationService(context);

            await Assert.ThrowsAsync<DuplicateRegistrationException>(() =>
                service.RegisterAsync(1, 1));
        }

        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEventIsFull()
        {
            await using var context = TestDbContext.CreateContext();

            context.Participants.Add(new Participant
            {
                Id = 2,
                FullName = "New Participant",
                Email = "new@test.com",
                PhoneNumber = "9800000000"
            });

            context.CommunityEvents.Add(new CommunityEvent
            {
                Id = 1,
                EventName = "Full Event",
                Description = "Full event",
                EventDate = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Capacity = 1
            });

            context.Registrations.Add(new Registration
            {
                Id = 1,
                ParticipantId = 1,
                CommunityEventId = 1,
                RegistrationDate = DateTime.Now,
                Status = "Registered"
            });

            await context.SaveChangesAsync();

            var service = new RegistrationService(context);

            await Assert.ThrowsAsync<EventFullException>(() =>
                service.RegisterAsync(2, 1));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteRegistration()
        {
            await using var context = TestDbContext.CreateContext();

            context.Registrations.Add(new Registration
            {
                Id = 1,
                ParticipantId = 1,
                CommunityEventId = 1,
                RegistrationDate = DateTime.Now,
                Status = "Registered"
            });

            await context.SaveChangesAsync();

            var service = new RegistrationService(context);

            await service.DeleteAsync(1);

            var registrations = await service.GetAllAsync();

            Assert.Empty(registrations);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnRegistrations()
        {
            await using var context = TestDbContext.CreateContext();

            context.Participants.Add(new Participant
            {
                Id = 1,
                FullName = "Test Participant",
                Email = "test@test.com",
                PhoneNumber = "9800000000"
            });

            context.CommunityEvents.Add(new CommunityEvent
            {
                Id = 1,
                EventName = "Test Event",
                Description = "Event description",
                EventDate = DateTime.Today.AddDays(1),
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 0, 0),
                Capacity = 5
            });

            context.Registrations.Add(new Registration
            {
                Id = 1,
                ParticipantId = 1,
                CommunityEventId = 1,
                RegistrationDate = DateTime.Now,
                Status = "Registered"
            });

            await context.SaveChangesAsync();

            var service = new RegistrationService(context);

            var registrations = await service.GetAllAsync();

            Assert.Single(registrations);
        }
    }
}