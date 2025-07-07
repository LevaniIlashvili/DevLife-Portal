namespace DevLifePortal.Domain.Entities
{
    public class GithubPersonalityProfile
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public List<string> Strengths { get; set; }
        public List<string> Weaknesses { get; set; }
        public string CelebrityMatch { get; set; }
    }
}