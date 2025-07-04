namespace DevLifePortal.Domain.Entities
{
    public class Excuse
    {
        public Guid Id { get; set; } 
        public string Category { get; set; }
        public string Type { get; set; }    
        public string Text { get; set; }
    }
}
