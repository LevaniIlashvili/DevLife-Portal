using System.Text.Json.Serialization;

namespace DevLifePortal.Infrastructure.Services.Github.Models
{
    public class GithubLoginResponse
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }
    }
}
