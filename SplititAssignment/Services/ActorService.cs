using AutoMapper;
using SplititAssignment.Exceptions;
using SplititAssignment.Interfaces;
using SplititAssignment.Models;

namespace SplititAssignment.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;

        public ActorService(IActorRepository actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }

        public ActorResponse AddActor(ActorRequest request)
        {
            var currentRankId = _actorRepository.GetRankId(request.Rank, request.Source);

            if (!string.IsNullOrEmpty(currentRankId))
            {
                throw new DuplicateEntityException("Rank", request.Rank.ToString(), request.Source);
            }

            if (_actorRepository.NameExists(request.Name, request.Source))
            {
                throw new DuplicateEntityException("Name", request.Name, request.Source);
            }

            var actor = _mapper.Map<Actor>(request);

            var response = _mapper.Map<ActorResponse>(actor);
            return response;
        }

        public void DeleteActor(string actorId)
        {
            var toRemove = _actorRepository.GetActor(actorId);

            if (toRemove == null)
            {
                throw new ValidationException("No matching ID found for removal.");
            }

            _actorRepository.DeleteActor(toRemove);
        }

        public ActorResponse GetActorDetails(string actorId)
        {
            var actor = _actorRepository.GetActor(actorId);

            if (actor == null)
            {
                throw new ValidationException("No matching ID found.");
            }

            return _mapper.Map<ActorResponse>(actor);
        }

        public ActorsSummaryResponse GetActorsSummary(string provider, int? rankStart = null, int? rankEnd = null, int skip = 0, int take = 10)
        {
            var actors = _actorRepository.GetActors(provider, rankStart, rankEnd, skip, take);

            return new ActorsSummaryResponse { Actors = _mapper.Map<IEnumerable<ActorSummary>>(actors) };
        }

        public ActorResponse UpdateActor(string actorId, ActorRequest request)
        {
            var existingActor = GetActorDetails(actorId);

            if (existingActor == null)
            {
                throw new ValidationException("No matching ID found for update.");
            }

            var currentRankId = _actorRepository.GetRankId(request.Rank, request.Source);

            if (!string.IsNullOrEmpty(currentRankId) && currentRankId != actorId)
            {
                throw new DuplicateEntityException("Rank" ,request.Rank.ToString(), request.Source);
            }

            var actor = _mapper.Map<Actor>(request);

            actor.Id = actorId;
            _actorRepository.Update(actor);
            return _mapper.Map<ActorResponse>(actor);
        }
    }
}
