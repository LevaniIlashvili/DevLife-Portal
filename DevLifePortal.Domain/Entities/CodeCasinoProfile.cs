namespace DevLifePortal.Domain.Entities
{
    public class CodeCasinoProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }

        public User User { get; set; }
    }
}
