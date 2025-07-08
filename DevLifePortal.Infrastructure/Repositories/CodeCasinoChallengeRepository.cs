using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class CodeCasinoChallengeRepository : ICodeCasinoChallengeRepository
    {
        private readonly IMongoCollection<CodeCasinoChallenge> _challengeCollection;

        public CodeCasinoChallengeRepository(IMongoClient mongoClient, IConfiguration configuration)
        {
            var dbName = configuration.GetConnectionString("MongoDb");
            var db = mongoClient.GetDatabase("DevLife");
            _challengeCollection = db.GetCollection<CodeCasinoChallenge>("code_casino_challenges");
        }

        public async Task<List<CodeCasinoChallenge>> GetAllAsync()
        {
            return await _challengeCollection
                .Find(_ => true)
                .Project(x => new CodeCasinoChallenge
                {
                    CorrectCode = x.CorrectCode,
                    IncorrectCode = x.IncorrectCode,
                    TechStack = x.TechStack
                })
                .ToListAsync();
        }
    }
}
