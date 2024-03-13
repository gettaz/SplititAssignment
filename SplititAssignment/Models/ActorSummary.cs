namespace SplititAssignment.Models
{
    public class ActorSummary
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ActorSummary(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
