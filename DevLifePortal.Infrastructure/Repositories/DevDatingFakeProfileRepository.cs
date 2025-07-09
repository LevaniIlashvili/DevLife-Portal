using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using MongoDB.Driver;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class DevDatingFakeProfileRepository : IDevDatingFakeProfileRepository
    {
        private readonly IMongoCollection<DevDatingFakeProfile> _profileCollection;

        public DevDatingFakeProfileRepository(IMongoCollection<DevDatingFakeProfile> collection)
        {
            _profileCollection = collection;
        }

        public async Task<List<DevDatingFakeProfile>> GetAllAsync()
        {
            return await _profileCollection.Find(_ => true).ToListAsync();
        }

        public async Task<DevDatingFakeProfile?> GetByIdAsync(Guid id)
        {
            var filter = Builders<DevDatingFakeProfile>.Filter.Eq(p => p.Id, id);
            return await _profileCollection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
