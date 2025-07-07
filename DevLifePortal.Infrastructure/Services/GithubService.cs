using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DevLifePortal.Infrastructure.Services
{
    public class GithubService : IGithubService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public GithubService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> ExchangeCodeForAccessTokenAsync(string code)
        {
            var clientId = _config["GitHubOAuth:ClientId"];
            var clientSecret = _config["GitHubOAuth:ClientSecret"];

            var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code }
            });

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<JsonElement>(json);
            return obj.GetProperty("access_token").GetString();
        }

        public async Task<string> GetGitHubUsernameAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("DevLifePortal", "1.0"));

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<JsonElement>(json);

            return obj.GetProperty("login").GetString();
        }

        public async Task<List<string>> GetUserRepositoriesAsync(string accessToken)
        {
            var query = new
            {
                query = @"
                {
                  viewer {
                    repositories(first: 100, orderBy: {field: UPDATED_AT, direction: DESC}) {
                      nodes {
                        name
                        nameWithOwner
                      }
                    }
                  }
                }"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/graphql");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.UserAgent.ParseAdd("DevLife-Portal");
            request.Content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var repos = doc.RootElement
                .GetProperty("data")
                .GetProperty("viewer")
                .GetProperty("repositories")
                .GetProperty("nodes")
                .EnumerateArray()
                .Select(repo => repo.GetProperty("nameWithOwner").GetString())
                .ToList();

            return repos;
        }

        public async Task<GithubRepoAnalysisResult> AnalyzeRepositoryAsync(string accessToken, string fullName)
        {
            var parts = fullName.Split('/');
            var owner = parts[0];
            var name = parts[1];

            var query = new
            {
                query = $@"
                {{
                  repository(owner: ""{owner}"", name: ""{name}"") {{
                    name
                    object(expression: ""HEAD:"") {{
                      ... on Tree {{
                        entries {{
                          name
                          type
                        }}
                      }}
                    }}
                    defaultBranchRef {{
                      target {{
                        ... on Commit {{
                          history(first: 10) {{
                            edges {{
                              node {{
                                message
                              }}
                            }}
                          }}
                        }}
                      }}
                    }}
                  }}
                }}"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/graphql");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.UserAgent.ParseAdd("DevLife-Portal");
            request.Content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement.GetProperty("data").GetProperty("repository");

            var files = root
                .GetProperty("object")
                .GetProperty("entries")
                .EnumerateArray()
                .Where(e => e.GetProperty("type").GetString() == "blob")
                .Select(e => e.GetProperty("name").GetString())
                .ToList();

            var commits = root
                .GetProperty("defaultBranchRef")
                .GetProperty("target")
                .GetProperty("history")
                .GetProperty("edges")
                .EnumerateArray()
                .Select(e => e.GetProperty("node").GetProperty("message").GetString())
                .ToList();

            var personality = AnalyzePersonality(files, commits);

            return new GithubRepoAnalysisResult
            {
                RepoName = fullName,
                TopLevelFiles = files,
                RecentCommitMessages = commits,
                Personality = personality
            };
        }

        private GithubPersonalityProfile AnalyzePersonality(List<string> files, List<string> commits)
        {
            bool hasReadme = files.Any(f => f.ToLower().Contains("readme"));
            bool hasTests = files.Any(f => f.ToLower().Contains("test") || f.ToLower().Contains("spec"));
            bool hasDocs = files.Any(f => f.ToLower().Contains("doc"));
            bool hasUtils = files.Any(f => f.ToLower().Contains("util") || f.ToLower().Contains("helper"));

            int shortCommits = commits.Count(c => c.Length < 20);
            int emojiCommits = commits.Count(c => c.Contains("🔥") || c.Contains("🚀") || c.Contains("✨"));

            if (hasTests && hasDocs && emojiCommits > 2)
            {
                return new GithubPersonalityProfile
                {
                    Type = "The Perfectionist",
                    Description = "You document, test, and emoji your way to success. Structure is your sanctuary.",
                    Strengths = new List<string> { "Detailed", "Disciplined", "Reliable" },
                    Weaknesses = new List<string> { "Overengineers", "Gets lost in documentation" },
                    CelebrityMatch = "Uncle Bob (Robert C. Martin)"
                };
            }

            if (shortCommits > 5 && !hasDocs)
            {
                return new GithubPersonalityProfile
                {
                    Type = "The Hacker",
                    Description = "You move fast, break things, and rarely look back. Code is your playground.",
                    Strengths = new List<string> { "Creative", "Fast", "Fearless" },
                    Weaknesses = new List<string> { "Lacks structure", "Forgets documentation" },
                    CelebrityMatch = "Linus Torvalds"
                };
            }

            if (hasReadme && hasUtils)
            {
                return new GithubPersonalityProfile
                {
                    Type = "The Architect",
                    Description = "You build for the future. Utilities, structure, and modularity define your world.",
                    Strengths = new List<string> { "Strategic thinker", "Scales well", "Organized" },
                    Weaknesses = new List<string> { "May overcomplicate early", "Sometimes abstract" },
                    CelebrityMatch = "Martin Fowler"
                };
            }

            return new GithubPersonalityProfile
            {
                Type = "The Pragmatist",
                Description = "You do what works. Not too much, not too little.",
                Strengths = new List<string> { "Efficient", "Practical", "Focused" },
                Weaknesses = new List<string> { "May miss edge cases", "Documentation optional" },
                CelebrityMatch = "Kent Beck"
            };
        }
    }
}
