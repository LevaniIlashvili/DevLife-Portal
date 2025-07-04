namespace DevLifePortal.Domain.Entities
{
    public class BugChaseProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MaxScore { get; set; }

        public User User { get; set; }
    }
}
