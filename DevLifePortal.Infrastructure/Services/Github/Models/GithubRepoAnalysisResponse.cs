using System.Text.Json.Serialization;

namespace DevLifePortal.Infrastructure.Services.Github.Models
{
    public class GithubRepoAnalysisResponse
    {
        [JsonPropertyName("data")]
        public RepoAnalysisData Data { get; set; }
    }

    public class RepoAnalysisData
    {
        [JsonPropertyName("repository")]
        public RepoAnalysisRepository Repository { get; set; }
    }

    public class RepoAnalysisRepository
    {
        [JsonPropertyName("object")]
        public RepoAnalysisObject Object { get; set; }

        [JsonPropertyName("defaultBranchRef")]
        public RepoAnalysisBranchRef DefaultBranchRef { get; set; }
    }

    public class RepoAnalysisObject
    {
        [JsonPropertyName("entries")]
        public List<RepoAnalysisEntry> Entries { get; set; }
    }

    public class RepoAnalysisEntry
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class RepoAnalysisBranchRef
    {
        [JsonPropertyName("target")]
        public RepoAnalysisTarget Target { get; set; }
    }

    public class RepoAnalysisTarget
    {
        [JsonPropertyName("history")]
        public RepoAnalysisHistory History { get; set; }
    }

    public class RepoAnalysisHistory
    {
        [JsonPropertyName("edges")]
        public List<RepoAnalysisEdge> Edges { get; set; }
    }

    public class RepoAnalysisEdge
    {
        [JsonPropertyName("node")]
        public RepoAnalysisNode Node { get; set; }
    }

    public class RepoAnalysisNode
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
