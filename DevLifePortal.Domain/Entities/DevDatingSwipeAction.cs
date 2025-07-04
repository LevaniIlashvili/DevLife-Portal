namespace DevLifePortal.Domain.Entities
{
    public class DevDatingSwipeAction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int UserId { get; set; }
        public Guid TargetProfileId { get; set; }
        public string Direction { get; set; } = string.Empty;
    }
}
