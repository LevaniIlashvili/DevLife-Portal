using Bogus;
using DevLifePortal.Domain.Entities;
using MongoDB.Driver;

namespace DevLifePortal.Infrastructure.Mongo
{
    public class Seeders
    {
        public class CodeCasinoChallengeSeeder
        {
            private readonly IMongoCollection<CodeCasinoChallenge> _collection;

            public CodeCasinoChallengeSeeder(IMongoCollection<CodeCasinoChallenge> collection)
            {
                _collection = collection;
            }

            public async Task SeedAsync()
            {
                var count = await _collection.CountDocumentsAsync(FilterDefinition<CodeCasinoChallenge>.Empty);
                if (count > 0) return;

                var challenges = new List<CodeCasinoChallenge>
{
                    new()
                    {
                        TechStack = ".NET",
                        CorrectCode = "var result = await dbContext.Users.ToListAsync();",
                        IncorrectCode = "var result = dbContext.Users.ToList();"
                    },
                    new()
                    {
                        TechStack = "JavaScript",
                        CorrectCode = "const sum = (a, b) => a + b;",
                        IncorrectCode = "const sum = a, b => a + b;"
                    },
                    new()
                    {
                        TechStack = "Python",
                        CorrectCode = "def greet():\n    print(\"Hello\")",
                        IncorrectCode = "def greet()\n    print(\"Hello\")"
                    },
                    new()
                    {
                        TechStack = "Java",
                        CorrectCode = "List<String> names = new ArrayList<>();",
                        IncorrectCode = "List<String> names = new ArrayList();"
                    },
                    new()
                    {
                        TechStack = "C++",
                        CorrectCode = "std::vector<int> nums = {1, 2, 3};",
                        IncorrectCode = "vector<int> nums = {1, 2, 3};"
                    },
                    new()
                    {
                        TechStack = "Ruby",
                        CorrectCode = "puts 'Hello, world!'",
                        IncorrectCode = "print('Hello, world!')"
                    },
                    new()
                    {
                        TechStack = "Go",
                        CorrectCode = "fmt.Println(\"Hello, Go\")",
                        IncorrectCode = "fmt.print(\"Hello, Go\")"
                    },
                    new()
                    {
                        TechStack = "TypeScript",
                        CorrectCode = "let name: string = 'Alice';",
                        IncorrectCode = "let name = string 'Alice';"
                    },
                    new()
                    {
                        TechStack = "Kotlin",
                        CorrectCode = "val name: String = \"Kotlin\"",
                        IncorrectCode = "val name = String: \"Kotlin\""
                    },
                    new()
                    {
                        TechStack = "Rust",
                        CorrectCode = "let x: i32 = 5;",
                        IncorrectCode = "let x = i32: 5;"
                    },
                    new()
                    {
                        TechStack = "Swift",
                        CorrectCode = "let greeting = \"Hello\"",
                        IncorrectCode = "let greeting: = \"Hello\""
                    }
                };


                await _collection.InsertManyAsync(challenges);
            }
        }

        public class DevDatingFakeProfileSeeder
        {
            private readonly IMongoCollection<DevDatingFakeProfile> _collection;

            public DevDatingFakeProfileSeeder(IMongoCollection<DevDatingFakeProfile> collection)
            {
                _collection = collection;
            }

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

            public async Task SeedAsync()
            {
                if (await _collection.CountDocumentsAsync(FilterDefinition<DevDatingFakeProfile>.Empty) > 0)
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

                await _collection.InsertManyAsync(profiles);
            }
        }
    }
}
