using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using MongoDB.Driver;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class DevDateSwipeRepository : IDevDateSwipeRepository
    {
        private readonly IMongoCollection<DevDatingSwipeAction> _swipeCollection;

        public DevDateSwipeRepository(IMongoCollection<DevDatingSwipeAction> collection)
        {
            _swipeCollection = collection;
        }

        public async Task SaveSwipeAsync(DevDatingSwipeAction swipe)
        {
            await _swipeCollection.InsertOneAsync(swipe);
        }

        public async Task<List<Guid>> GetSwipedFakeProfileIdsAsync(int userId)
        {
            var filter = Builders<DevDatingSwipeAction>.Filter.Eq(s => s.UserId, userId);
            var projection = Builders<DevDatingSwipeAction>.Projection.Include(s => s.TargetProfileId);

            var swipes = await _swipeCollection
                .Find(filter)
                .Project<DevDatingSwipeAction>(projection)
                .ToListAsync();

            return swipes.Select(s => s.TargetProfileId).ToList();
        }

        public async Task<List<Guid>> GetRightSwipedFakeProfileIdsAsync(int userId)
        {
            var filter = Builders<DevDatingSwipeAction>.Filter.And(
                Builders<DevDatingSwipeAction>.Filter.Eq(s => s.UserId, userId),
                Builders<DevDatingSwipeAction>.Filter.Eq(s => s.Direction, "right")
            );

            var projection = Builders<DevDatingSwipeAction>.Projection.Include(s => s.TargetProfileId);

            var swipes = await _swipeCollection
                .Find(filter)
                .Project<DevDatingSwipeAction>(projection)
                .ToListAsync();

            return swipes.Select(s => s.TargetProfileId).ToList();
        }
    }
}
