using CommunityEvent_ManagementSystem.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvent_ManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CommunityEvent> CommunityEvents { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<EventVenue> EventVenues { get; set; }
        public DbSet<EventActivity> EventActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Registration>()
                .HasOne(r => r.Participant)
                .WithMany(p => p.Registrations)
                .HasForeignKey(r => r.ParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Registration>()
                .HasOne(r => r.CommunityEvent)
                .WithMany(e => e.Registrations)
                .HasForeignKey(r => r.CommunityEventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EventVenue>()
                .HasOne(ev => ev.CommunityEvent)
                .WithMany(e => e.EventVenues)
                .HasForeignKey(ev => ev.CommunityEventId);

            builder.Entity<EventVenue>()
                .HasOne(ev => ev.Venue)
                .WithMany(v => v.EventVenues)
                .HasForeignKey(ev => ev.VenueId);

            builder.Entity<EventActivity>()
                .HasOne(ea => ea.CommunityEvent)
                .WithMany(e => e.EventActivities)
                .HasForeignKey(ea => ea.CommunityEventId);

            builder.Entity<EventActivity>()
                .HasOne(ea => ea.Activity)
                .WithMany(a => a.EventActivities)
                .HasForeignKey(ea => ea.ActivityId);
        }
    }
}