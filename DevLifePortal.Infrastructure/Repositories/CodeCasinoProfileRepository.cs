using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class CodeCasinoProfileRepository : ICodeCasinoProfileRepository
    {
        private readonly DevLifeDbContext _dbContext;

        public CodeCasinoProfileRepository(DevLifeDbContext devLifeDbContext)
        {
            _dbContext = devLifeDbContext;
        }

        public async Task CreateProfile(int userId)
        {
            var profile = new CodeCasinoProfile { UserId = userId, Points = 100 };
            await _dbContext.CodeCasinoProfiles.AddAsync(profile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CodeCasinoProfile> GetProfile(int userId)
        {
            var profile = await _dbContext.CodeCasinoProfiles.FirstAsync(profile => profile.UserId == userId);
            return profile;
        }

        public async Task UpdateProfile(CodeCasinoProfile profile)
        {
            var profileToUpdate = await _dbContext.CodeCasinoProfiles.FindAsync(profile.Id);
            profileToUpdate.Points = profile.Points;
            await _dbContext.SaveChangesAsync();
        }
    }
}
