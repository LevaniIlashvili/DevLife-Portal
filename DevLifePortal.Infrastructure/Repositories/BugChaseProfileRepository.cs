using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using DevLifePortal.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class BugChaseProfileRepository : IBugChaseProfileRepository
    {
        private readonly DevLifeDbContext _dbContext;

        public BugChaseProfileRepository(DevLifeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BugChaseProfile> GetProfile(int userId)
        {
            var profile = await _dbContext.BugChaseProfiles.FirstAsync(p => p.UserId == userId);
            return profile;
        }

        public async Task<List<BugChaseProfile>> GetTopProfiles()
        {
            var profiles = await _dbContext.BugChaseProfiles.OrderByDescending(p => p.MaxScore).Take(10).ToListAsync();

            return profiles;
        }

        public async Task<BugChaseProfile> CreateProfile(int userId)
        {
            var profile = new BugChaseProfile()
            {
                UserId = userId,
                MaxScore = 0
            };
            await _dbContext.BugChaseProfiles.AddAsync(profile);
            await _dbContext.SaveChangesAsync();

            return profile;
        }

        public async Task UpdateProfile(BugChaseProfile bugChaseProfile)
        {
            var profile = await _dbContext.BugChaseProfiles.FirstAsync(p => p.UserId == bugChaseProfile.UserId);
            profile.MaxScore = bugChaseProfile.MaxScore;

            await _dbContext.SaveChangesAsync();
        }
    }
}
