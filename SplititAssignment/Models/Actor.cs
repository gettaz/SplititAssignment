namespace SplititAssignment.Models
{
    public class Actor
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Details { get; set; }
        public string Type { get; set; }
        public int Rank { get; set; }
        public string Source { get; set; }
    }
}
