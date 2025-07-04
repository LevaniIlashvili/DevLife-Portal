using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class DevDatingFakeProfileRepository : IDevDatingFakeProfileRepository
    {
        private readonly IMongoCollection<DevDatingFakeProfile> _profileCollection;

        public DevDatingFakeProfileRepository(IMongoClient mongoClient, IConfiguration configuration)
        {
            var dbName = configuration.GetConnectionString("MongoDb");
            var db = mongoClient.GetDatabase("DevLife");
            _profileCollection = db.GetCollection<DevDatingFakeProfile>("fake_profiles");
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
