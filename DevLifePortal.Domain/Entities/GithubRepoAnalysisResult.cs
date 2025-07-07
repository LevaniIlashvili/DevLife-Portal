namespace DevLifePortal.Domain.Entities
{
    public class GithubRepoAnalysisResult
    {
        public string RepoName { get; set; }
        public List<string> TopLevelFiles { get; set; }
        public List<string> RecentCommitMessages { get; set; }
        public GithubPersonalityProfile Personality { get; set; }
    }
}