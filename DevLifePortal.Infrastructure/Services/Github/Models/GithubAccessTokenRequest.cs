namespace DevLifePortal.Infrastructure.Services.Github.Models
{
    public class GithubAccessTokenRequest
    {
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Code { get; }

        public GithubAccessTokenRequest(string clientId, string clientSecret, string code)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Code = code;
        }

        public FormUrlEncodedContent ToFormContent()
        {
            return new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "code", Code }
            });
        }

    }
}
