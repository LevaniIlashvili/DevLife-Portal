namespace DevLifePortal.Domain.Entities
{
    public class DevDatingFakeProfile
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Preference { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string TechStack { get; set; } = string.Empty;
    }
}
