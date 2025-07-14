using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Domain.Entities;
using DevLifePortal.Infrastructure.Configuration;
using DevLifePortal.Infrastructure.Services.Github.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DevLifePortal.Infrastructure.Services.Github
{
    public class GithubService : IGithubService
    {
        private readonly HttpClient _httpClient;
        private readonly GitHubOAuthOptions _githubOAuthOptions;

        public GithubService(HttpClient httpClient, IOptions<GitHubOAuthOptions> githubOAuthOptions)
        {
            _httpClient = httpClient;
            _githubOAuthOptions = githubOAuthOptions.Value;
        }

        public async Task<string> ExchangeCodeForAccessTokenAsync(string code)
        {
            var githubTokenRequest = new GithubAccessTokenRequest(
                _githubOAuthOptions.ClientId,
                _githubOAuthOptions.ClientSecret,
                code);

            var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = githubTokenRequest.ToFormContent();

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<GithubAccessTokenResponse>(json);
            return obj.AccessToken;
        }

        public async Task<string> GetGitHubUsernameAsync(string accessToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonSerializer.Deserialize<GithubLoginResponse>(json);

            return obj.Login;
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
            request.Content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GithubRepositoriesResponse>(json);

            var repos = result.Data.Viewer.Repositories.Nodes
                .Select(n => n.NameWithOwner)
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
            request.Content = new StringContent(JsonSerializer.Serialize(query), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GithubRepoAnalysisResponse>(json);

            var files = result.Data.Repository.Object.Entries
                .Where(e => e.Type == "blob")
                .Select(e => e.Name)
                .ToList();

            var commits = result.Data.Repository.DefaultBranchRef.Target.History.Edges
                .Select(e => e.Node.Message)
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
