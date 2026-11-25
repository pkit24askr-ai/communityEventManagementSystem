using CommunityEvent_ManagementSystem.Data;
using CommunityEvent_ManagementSystem.Data.Models;
using CommunityEvent_ManagementSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvent_ManagementSystem.Services
{
    public class VenueService : IVenueService
    {
        private readonly ApplicationDbContext _context;

        public VenueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Venue>> GetAllAsync()
        {
            return await _context.Venues.OrderBy(v => v.VenueName).ToListAsync();
        }

        public async Task<Venue?> GetByIdAsync(int id)
        {
            return await _context.Venues.FindAsync(id);
        }

        public async Task AddAsync(Venue venue)
        {
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Venue venue)
        {
            var existingVenue = await _context.Venues
                .FirstOrDefaultAsync(v => v.Id == venue.Id);

            if (existingVenue == null)
                throw new InvalidOperationException("Venue not found.");

            existingVenue.VenueName = venue.VenueName;
            existingVenue.Address = venue.Address;
            existingVenue.Capacity = venue.Capacity;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var venue = await _context.Venues
                .Include(v => v.EventVenues)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (venue == null)
                throw new InvalidOperationException("Venue not found.");

            if (venue.EventVenues.Any())
                throw new InvalidOperationException(
                    "Cannot delete this venue because it is assigned to one or more events.");

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
        }
    }
}