using DevLifePortal.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace DevLifePortal.Infrastructure.Mongo
{
    public static class MongoClassMapping
    {
        public static void RegisterMappings()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(DevDatingFakeProfile)))
            {
                BsonClassMap.RegisterClassMap<DevDatingFakeProfile>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdProperty(c => c.Id)
                      .SetSerializer(new GuidSerializer(BsonType.String));
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(DevDatingSwipeAction)))
            {
                BsonClassMap.RegisterClassMap<DevDatingSwipeAction>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id)
                      .SetIdGenerator(GuidGenerator.Instance)
                      .SetSerializer(new GuidSerializer(BsonType.String));
                    cm.MapMember(c => c.TargetProfileId)
                      .SetSerializer(new GuidSerializer(BsonType.String));
                });
            }

        }
    }
}
