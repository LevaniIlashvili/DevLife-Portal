using DevLifePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevLifePortal.Infrastructure
{
    public class DevLifeDbContext : DbContext  
    {
        public DevLifeDbContext(DbContextOptions<DevLifeDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<CodeCasinoProfile> CodeCasinoProfiles => Set<CodeCasinoProfile>();
        public DbSet<BugChaseProfile> BugChaseProfiles => Set<BugChaseProfile>();
    }
}
