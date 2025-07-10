namespace DevLifePortal.Infrastructure.Configuration
{
    public class GitHubOAuthOptions
    {
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public string CallbackUrl { get; set; } = "";
    }
}
