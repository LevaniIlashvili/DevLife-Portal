using Bogus;
using DevLifePortal.Domain.Entities;
using MongoDB.Driver;

namespace DevLifePortal.Infrastructure.Mongo
{
    public class StaticDatingProfileSeeder
    {
        private static readonly string[] Genders = new[] { "Male", "Female" };
        private static readonly string[] Preferences = new[] { "Male", "Female" };
        private static readonly string[] TechStacks = new[] {
            "C#", "JavaScript", "Python", "Go", "Java", "TypeScript",
            "React", "Vue", "Angular", "MongoDB", "SQL", "Redis",
            "Docker", "Kubernetes", "Node.js"
        };
        private static readonly string[] Bios = new[] {
            "I code better when I'm in love 💻❤️",
            "Looking for someone to merge PRs and lives 👩‍💻👨‍💻",
            "Introvert IRL, extrovert in the terminal 🤓",
            "Let’s talk design patterns and dating patterns 😉",
            "Coffee, code & cuddles ☕💻🤗"
        };

        public async Task SeedAsync(IMongoCollection<DevDatingFakeProfile> collection)
        {
            if (await collection.CountDocumentsAsync(FilterDefinition<DevDatingFakeProfile>.Empty) > 0)
                return;

            var faker = new Faker();
            var profiles = new List<DevDatingFakeProfile>();

            for (int i = 0; i < 100; i++)
            {
                var name = faker.Name.FirstName();
                var gender = faker.PickRandom(Genders);
                var preference = faker.PickRandom(Preferences);
                var techStack = faker.PickRandom(TechStacks);
                var avatarSeed = faker.Random.Guid().ToString();

                profiles.Add(new DevDatingFakeProfile
                {
                    Name = name,
                    Gender = gender,
                    Preference = preference,
                    Bio = faker.PickRandom(Bios),
                    TechStack = techStack,
                });
            }

            await collection.InsertManyAsync(profiles);
        }
    }
}
