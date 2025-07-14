using System.Text.Json.Serialization;

namespace DevLifePortal.Infrastructure.Services.Github.Models
{
    public class GithubRepositoriesResponse
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("viewer")]
        public Viewer Viewer { get; set; }
    }

    public class Viewer
    {
        [JsonPropertyName("repositories")]
        public RepositoryConnection Repositories { get; set; }
    }

    public class RepositoryConnection
    {
        [JsonPropertyName("nodes")]
        public List<RepositoryNode> Nodes { get; set; }
    }

    public class RepositoryNode
    {
        [JsonPropertyName("nameWithOwner")]
        public string NameWithOwner { get; set; }
    }
}
