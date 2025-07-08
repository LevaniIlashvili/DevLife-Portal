namespace DevLifePortal.Application.DTOs
{
    public class DevDatingProfileDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsMale { get; set; }
        public bool PrefersMale { get; set; }
        public string Bio { get; set; }
    }
}
