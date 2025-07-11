using DevLifePortal.Domain.Entities;
using DevLifePortal.Infrastructure.Postgres.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DevLifePortal.Infrastructure.Postgres
{
    public class DevLifeDbContext : DbContext  
    {
        public DevLifeDbContext(DbContextOptions<DevLifeDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<CodeCasinoProfile> CodeCasinoProfiles => Set<CodeCasinoProfile>();
        public DbSet<BugChaseProfile> BugChaseProfiles => Set<BugChaseProfile>();
        public DbSet<DevDatingProfile> DevDatingProfiles => Set<DevDatingProfile>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfigurations).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
