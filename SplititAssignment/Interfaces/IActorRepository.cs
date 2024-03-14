using SplititAssignment.Models;

namespace SplititAssignment.Interfaces
{
    public interface IActorRepository
    {
        IEnumerable<Actor> GetActors(string provider, int? rankStart = null, int? rankEnd = null, int skip = 0, int take = 10);
        void AddActors (IEnumerable<Actor> actors);
        string GetRankId (int rank, string provider);
        bool NameExists(string name, string provider);
        void AddActor (Actor actor);
        void DeleteActor (Actor actor);
        Actor GetActor (string actorId);
        void Update(Actor actor);
    }
}
