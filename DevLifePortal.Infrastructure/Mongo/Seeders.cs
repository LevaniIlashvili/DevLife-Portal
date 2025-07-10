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
                    // JavaScript
                    new()
                    {
                        TechStack = "Javascript",
                        CorrectCode = "const total = [1, 2, 3].reduce((acc, curr) => acc + curr, 0);",
                        IncorrectCode = "const total = [1, 2, 3].reduce(acc, curr => acc + curr, 0);"
                    },
                    new()
                    {
                        TechStack = "Javascript",
                        CorrectCode = "if (typeof value === 'string') { console.log(value); }",
                        IncorrectCode = "if (typeof value = 'string') { console.log(value); }"
                    },
                    new()
                    {
                        TechStack = "Javascript",
                        CorrectCode = "function greet(name) { return `Hello, ${name}`; }",
                        IncorrectCode = "function greet(name) return `Hello, ${name}`;"
                    },

                    // TypeScript
                    new()
                    {
                        TechStack = "Typescript",
                        CorrectCode = "interface User { name: string; age: number; }",
                        IncorrectCode = "interface User { name = string; age = number; }"
                    },
                    new()
                    {
                        TechStack = "Typescript",
                        CorrectCode = "let scores: number[] = [90, 80, 70];",
                        IncorrectCode = "let scores = number[] [90, 80, 70];"
                    },
                    new()
                    {
                        TechStack = "Typescript",
                        CorrectCode = "function getLength(item: string | string[]): number { return item.length; }",
                        IncorrectCode = "function getLength(item: string || string[]): number { return item.length; }"
                    },

                    // React
                    new()
                    {
                        TechStack = "React",
                        CorrectCode = "useEffect(() => { fetchData(); }, []);",
                        IncorrectCode = "useEffect(() => fetchData(), );"
                    },
                    new()
                    {
                        TechStack = "React",
                        CorrectCode = "return <div className=\"container\">Hello</div>;",
                        IncorrectCode = "return <div class=\"container\">Hello</div>;"
                    },
                    new()
                    {
                        TechStack = "React",
                        CorrectCode = "const handleClick = () => setCount(count + 1);",
                        IncorrectCode = "const handleClick = () => setCount = count + 1;"
                    },

                    // Angular
                    new()
                    {
                        TechStack = "Angular",
                        CorrectCode = "@Injectable({ providedIn: 'root' })",
                        IncorrectCode = "Injectable({ providedIn: 'root' })"
                    },
                    new()
                    {
                        TechStack = "Angular",
                        CorrectCode = "<button (click)=\"onClick()\">Click Me</button>",
                        IncorrectCode = "<button click=\"onClick()\">Click Me</button>"
                    },
                    new()
                    {
                        TechStack = "Angular",
                        CorrectCode = "ngOnInit(): void { this.loadData(); }",
                        IncorrectCode = "ngOnInit() void { this.loadData(); }"
                    },

                    // Net
                    new()
                    {
                        TechStack = "Net",
                        CorrectCode = "var user = await _dbContext.Users.FirstOrDefaultAsync();",
                        IncorrectCode = "var user = _dbContext.Users.FirstOrDefault();"
                    },
                    new()
                    {
                        TechStack = "Net",
                        CorrectCode = "[HttpGet(\"api/users/{id}\")]",
                        IncorrectCode = "HttpGet(\"api/users/{id}\")"
                    },
                    new()
                    {
                        TechStack = "Net",
                        CorrectCode = "public IActionResult Get() => Ok(_service.GetAll());",
                        IncorrectCode = "public ActionResult Get() => Ok(_service.GetAll());"
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
