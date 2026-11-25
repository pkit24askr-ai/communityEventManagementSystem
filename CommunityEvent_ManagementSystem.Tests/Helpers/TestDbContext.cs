using CommunityEvent_ManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvent_ManagementSystem.Tests.Helpers
{
    public static class TestDbContext
    {
        public static ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}