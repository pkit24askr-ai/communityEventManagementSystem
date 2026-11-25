using CommunityEvent_ManagementSystem.Data;
using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Interfaces;
using CommunityEvent_ManagementSystem.Validators;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvent_ManagementSystem.Services
{
    public class EventService : IEventService
    {
        private readonly ApplicationDbContext _context;

        public EventService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommunityEvent>> GetAllAsync()
        {
            return await _context.CommunityEvents
                .Include(e => e.EventVenues)
                    .ThenInclude(ev => ev.Venue)
                .Include(e => e.EventActivities)
                    .ThenInclude(ea => ea.Activity)
                .Include(e => e.Registrations)
                .OrderBy(e => e.EventDate)
                .ToListAsync();
        }

        public async Task<CommunityEvent?> GetByIdAsync(int id)
        {
            return await _context.CommunityEvents
                .Include(e => e.EventVenues)
                    .ThenInclude(ev => ev.Venue)
                .Include(e => e.EventActivities)
                    .ThenInclude(ea => ea.Activity)
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task AddAsync(CommunityEvent eventItem)
        {
            EventValidator.Validate(eventItem);

            _context.CommunityEvents.Add(eventItem);
            await _context.SaveChangesAsync();

            foreach (var venueId in eventItem.SelectedVenueIds)
            {
                _context.EventVenues.Add(new EventVenue
                {
                    CommunityEventId = eventItem.Id,
                    VenueId = venueId
                });
            }

            foreach (var activityId in eventItem.SelectedActivityIds)
            {
                _context.EventActivities.Add(new EventActivity
                {
                    CommunityEventId = eventItem.Id,
                    ActivityId = activityId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CommunityEvent eventItem)
        {
            EventValidator.Validate(eventItem);

            _context.CommunityEvents.Update(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var eventItem = await _context.CommunityEvents
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
                throw new InvalidOperationException("Event not found.");

            if (eventItem.Registrations.Any())
                throw new InvalidOperationException(
                    "Cannot delete an event that has participant registrations.");

            _context.CommunityEvents.Remove(eventItem);
            await _context.SaveChangesAsync();
        }

    }
}