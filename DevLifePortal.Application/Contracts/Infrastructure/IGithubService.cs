using DevLifePortal.Domain.Entities;

namespace DevLifePortal.Application.Contracts.Infrastructure
{
    public interface IGithubService
    {
        Task<string> ExchangeCodeForAccessTokenAsync(string code);
        Task<string> GetGitHubUsernameAsync(string accessToken);
        Task<List<string>> GetUserRepositoriesAsync(string accessToken);
        Task<GithubRepoAnalysisResult> AnalyzeRepositoryAsync(string accessToken, string fullName);
    }
}
