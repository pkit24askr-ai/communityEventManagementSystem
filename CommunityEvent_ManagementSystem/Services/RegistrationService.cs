using CommunityEvent_ManagementSystem.Data;
using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Exceptions;
using CommunityEvent_ManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvent_ManagementSystem.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _context;

        public RegistrationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Registration>> GetAllAsync()
        {
            return await _context.Registrations
                .Include(r => r.Participant)
                .Include(r => r.CommunityEvent)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();
        }

        public async Task RegisterAsync(int participantId, int eventId)
        {
            var alreadyRegistered = await _context.Registrations
                .AnyAsync(r => r.ParticipantId == participantId && r.CommunityEventId == eventId);

            if (alreadyRegistered)
                throw new DuplicateRegistrationException("Participant is already registered for this event.");

            var eventItem = await _context.CommunityEvents
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventItem == null)
                throw new Exception("Event not found.");

            if (eventItem.Registrations.Count >= eventItem.Capacity)
                throw new EventFullException("This event is already full.");

            var registration = new Registration
            {
                ParticipantId = participantId,
                CommunityEventId = eventId,
                RegistrationDate = DateTime.Now,
                Status = "Registered"
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registration == null)
                throw new InvalidOperationException("Registration not found.");

            _context.Registrations.Remove(registration);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Registration registration)
        {
            var existingRegistration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.Id == registration.Id);

            if (existingRegistration == null)
                throw new InvalidOperationException("Registration not found.");

            existingRegistration.Status = registration.Status;

            await _context.SaveChangesAsync();
        }
    }
}