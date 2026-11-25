using CommunityEvent_ManagementSystem.Data;
using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Interfaces;
using CommunityEvent_ManagementSystem.Validators;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvent_ManagementSystem.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly ApplicationDbContext _context;

        public ParticipantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Participant>> GetAllAsync()
        {
            return await _context.Participants
                .OrderBy(p => p.FullName)
                .ToListAsync();
        }

        public async Task<Participant?> GetByIdAsync(int id)
        {
            return await _context.Participants.FindAsync(id);
        }

        public async Task AddAsync(Participant participant)
        {
            ParticipantValidator.Validate(participant);

            var emailExists = await _context.Participants
                .AnyAsync(p => p.Email == participant.Email);

            if (emailExists)
                throw new InvalidOperationException("A participant with this email already exists.");

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Participant participant)
        {
            ParticipantValidator.Validate(participant);

            var emailExists = await _context.Participants
                .AnyAsync(p => p.Email == participant.Email && p.Id != participant.Id);

            if (emailExists)
                throw new InvalidOperationException("A participant with this email already exists.");

            var existingParticipant = await _context.Participants
                .FirstOrDefaultAsync(p => p.Id == participant.Id);

            if (existingParticipant == null)
                throw new InvalidOperationException("Participant not found.");

            existingParticipant.FullName = participant.FullName;
            existingParticipant.Email = participant.Email;
            existingParticipant.PhoneNumber = participant.PhoneNumber;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var participant = await _context.Participants
                .Include(p => p.Registrations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (participant == null)
                throw new InvalidOperationException("Participant not found.");

            if (participant.Registrations.Any())
                throw new InvalidOperationException(
                    "Cannot delete participant because they have registrations.");

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
        }
    }
}