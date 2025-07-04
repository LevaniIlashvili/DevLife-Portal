namespace DevLifePortal.Domain.Entities
{
    public class DevDatingProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsMale { get; set; }
        public bool PrefersMale { get; set; }
        public string Bio {  get; set; }

        public User User { get; set; }
    }
}
