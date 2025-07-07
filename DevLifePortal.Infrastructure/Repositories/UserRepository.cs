using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DevLifeDbContext _dbContext;

        public UserRepository(DevLifeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
