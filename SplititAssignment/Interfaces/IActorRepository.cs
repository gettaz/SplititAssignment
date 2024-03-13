using SplititAssignment.Models;

namespace SplititAssignment.Interfaces
{
    public interface IActorRepository
    {
        IEnumerable<Actor> GetActors(string provider, int? rankStart = null, int? rankEnd = null, int skip = 0, int take = 10);
        bool AddActors (IEnumerable<Actor> actors);
        bool RankExists (int rank, string provider);
        bool AddActor (Actor actor);
        bool DeleteActor (Actor actor);
        Actor GetActor (string actorId);
        bool Update(Actor actor);
    }
}
