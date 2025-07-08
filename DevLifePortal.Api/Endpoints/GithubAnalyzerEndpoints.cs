using DevLifePortal.Application.Contracts.Infrastructure;

namespace DevLifePortal.Api.Endpoints
{
    public static class GithubAnalyzerEndpoints
    {
        public static IEndpointRouteBuilder MapGithubAnalyzerEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/github/login", (HttpContext http, IConfiguration config) =>
            {
                var clientId = config["GitHubOAuth:ClientId"];
                var callbackUrl = config["GitHubOAuth:CallbackUrl"];
                var state = Guid.NewGuid().ToString();

                http.Session.SetString("OAuthState", state);

                var githubUrl = $"https://github.com/login/oauth/authorize?client_id={clientId}&redirect_uri={callbackUrl}&state={state}&scope=read:user%20repo";
                return Results.Redirect(githubUrl);
            })
            .WithTags("Github Analyzer");

            app.MapGet("/github/callback", async (string code, string state, HttpContext http, IGithubService githubService) =>
            {
                var expectedState = http.Session.GetString("OAuthState");
                if (state != expectedState)
                    return Results.BadRequest("Invalid OAuth state");

                var accessToken = await githubService.ExchangeCodeForAccessTokenAsync(code);
                var username = await githubService.GetGitHubUsernameAsync(accessToken);

                http.Session.SetString("GitHubAccessToken", accessToken);

                return Results.Ok(new { username, accessToken });
            })
            .WithTags("Github Analyzer");

            app.MapGet("/github/repos", async (HttpContext http, IGithubService gitHubService) =>
            {
                var accessToken = http.Session.GetString("GitHubAccessToken");
                if (string.IsNullOrEmpty(accessToken))
                    return Results.Unauthorized();

                var repos = await gitHubService.GetUserRepositoriesAsync(accessToken);
                return Results.Ok(repos);
            })
            .WithTags("Github Analyzer");

            app.MapGet("/github/analyze", async (
                HttpContext http,
                IGithubService githubService,
                string repoFullName) =>
            {
                var accessToken = http.Session.GetString("GitHubAccessToken");

                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(repoFullName))
                    return Results.Unauthorized();

                var analysis = await githubService.AnalyzeRepositoryAsync(accessToken, repoFullName);
                return Results.Ok(analysis);
            })
            .WithTags("Github Analyzer");

            return app;
        }
    }
}
