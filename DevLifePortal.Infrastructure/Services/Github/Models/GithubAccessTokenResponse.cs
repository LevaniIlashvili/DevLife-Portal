using System.Text.Json.Serialization;

namespace DevLifePortal.Infrastructure.Services.Github.Models
{
    public class GithubAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
