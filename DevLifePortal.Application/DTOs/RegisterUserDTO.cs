namespace DevLifePortal.Application.DTOs
{
    public class RegisterUserDTO
    {
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string TechStack { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
    }
}
