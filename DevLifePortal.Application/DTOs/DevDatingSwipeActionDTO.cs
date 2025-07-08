namespace DevLifePortal.Application.DTOs
{
    public class DevDatingSwipeActionDTO
    {
        public Guid TargetProfileId { get; set; }
        public string Direction { get; set; } = string.Empty;
    }
}
