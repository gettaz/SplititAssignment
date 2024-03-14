using SplititAssignment.Models;

namespace SplititAssignment.Interfaces
{
    public interface IActorService
    {
        ActorsSummaryResponse GetActorsSummary(string provider, int? rankStart = null, int? rankEnd = null, int skip = 0, int take = 10);
        ActorResponse GetActorDetails(string actorId);
        ActorResponse UpdateActor(string actorId, ActorRequest request);
        void DeleteActor(string actorId);
        ActorResponse AddActor(ActorRequest request);

    }
}
