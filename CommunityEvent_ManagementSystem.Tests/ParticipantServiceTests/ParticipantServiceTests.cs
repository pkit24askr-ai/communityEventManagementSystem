using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Services;
using CommunityEvent_ManagementSystem.Tests.Helpers;
using Xunit;

namespace CommunityEvent_ManagementSystem.Tests
{
    public class ParticipantServiceTests
    {
        [Fact]
        public async Task AddAsync_ShouldAddParticipant()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new ParticipantService(context);

            var participant = new Participant
            {
                FullName = "John Smith",
                Email = "john@test.com",
                PhoneNumber = "9800000000"
            };

            await service.AddAsync(participant);

            var participants = await service.GetAllAsync();

            Assert.Single(participants);
            Assert.Equal("John Smith", participants[0].FullName);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenEmailIsDuplicate()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new ParticipantService(context);

            await service.AddAsync(new Participant
            {
                FullName = "First User",
                Email = "same@test.com",
                PhoneNumber = "9800000000"
            });

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AddAsync(new Participant
                {
                    FullName = "Second User",
                    Email = "same@test.com",
                    PhoneNumber = "9811111111"
                }));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateParticipant()
        {
            await using var context = TestDbContext.CreateContext();
            var service = new ParticipantService(context);

            var participant = new Participant
            {
                FullName = "Old Name",
                Email = "old@test.com",
                PhoneNumber = "9800000000"
            };

            await service.AddAsync(participant);

            participant.FullName = "Updated Name";
            await service.UpdateAsync(participant);

            var updated = await service.GetByIdAsync(participant.Id);

            Assert.Equal("Updated Name", updated!.FullName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenParticipantHasRegistration()
        {
            await using var context = TestDbContext.CreateContext();

            context.Participants.Add(new Participant
            {
                Id = 1,
                FullName = "Registered User",
                Email = "registered@test.com",
                PhoneNumber = "9800000000"
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

            var service = new ParticipantService(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteAsync(1));
        }
    }
}