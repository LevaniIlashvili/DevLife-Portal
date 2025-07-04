using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class DevDatingProfileRepository : IDevDatingProfileRepository
    {
        private readonly DevLifeDbContext _dbContext;

        public DevDatingProfileRepository(DevLifeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DevDatingProfile?> GetAsync(int userId)
        {
            var profile = await _dbContext.DevDatingProfiles.FirstOrDefaultAsync(p => p.UserId ==  userId);
            return profile;
        }

        public async Task<DevDatingProfile> AddAsync(DevDatingProfile profile)
        {
            await _dbContext.AddAsync(profile);
            await _dbContext.SaveChangesAsync();
            return profile;
        }
    }
}
