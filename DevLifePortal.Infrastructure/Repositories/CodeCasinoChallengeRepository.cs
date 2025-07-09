using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using MongoDB.Driver;

namespace DevLifePortal.Infrastructure.Repositories
{
    public class CodeCasinoChallengeRepository : ICodeCasinoChallengeRepository
    {
        private readonly IMongoCollection<CodeCasinoChallenge> _challengeCollection;

        public CodeCasinoChallengeRepository(IMongoCollection<CodeCasinoChallenge> collection)
        {
            _challengeCollection = collection;
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
