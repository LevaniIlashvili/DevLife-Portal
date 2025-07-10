using DevLifePortal.Application.Contracts.Infrastructure;
using DevLifePortal.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace DevLifePortal.Api.Endpoints
{
    public static class GithubAnalyzerEndpoints
    {
        public static IEndpointRouteBuilder MapGithubAnalyzerEndpoints(this IEndpointRouteBuilder app)
        {
            var githubGroup = app.MapGroup("/github").WithTags("Github Analyzer");

            githubGroup.MapGet("/login", (HttpContext http, IOptions<GitHubOAuthOptions> gitHubOAuthOptions) =>
            {
                var clientId = gitHubOAuthOptions.Value.ClientId;
                var callbackUrl = gitHubOAuthOptions.Value.CallbackUrl;
                var state = Guid.NewGuid().ToString();

                http.Session.SetString("OAuthState", state);

                var githubUrl = $"https://github.com/login/oauth/authorize?client_id={clientId}&redirect_uri={callbackUrl}&state={state}&scope=read:user%20repo";
                return Results.Redirect(githubUrl);
            });

            githubGroup.MapGet("/callback", async (string code, string state, HttpContext http, IGithubService githubService) =>
            {
                var expectedState = http.Session.GetString("OAuthState");
                if (state != expectedState)
                    return Results.BadRequest("Invalid OAuth state");

                var accessToken = await githubService.ExchangeCodeForAccessTokenAsync(code);
                var username = await githubService.GetGitHubUsernameAsync(accessToken);

                http.Session.SetString("GitHubAccessToken", accessToken);

                return Results.Ok(new { username, accessToken });
            });

            githubGroup.MapGet("/repos", async (HttpContext http, IGithubService gitHubService) =>
            {
                var accessToken = http.Session.GetString("GitHubAccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    return Results.Unauthorized();

                var repos = await gitHubService.GetUserRepositoriesAsync(accessToken);
                return Results.Ok(repos);
            });

            githubGroup.MapGet("/analyze", async (
                HttpContext http,
                IGithubService githubService,
                string repoFullName) =>
            {
                var accessToken = http.Session.GetString("GitHubAccessToken");

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(repoFullName))
                    return Results.Unauthorized();

                var analysis = await githubService.AnalyzeRepositoryAsync(accessToken, repoFullName);
                return Results.Ok(analysis);
            });

            return app;
        }
    }
}
