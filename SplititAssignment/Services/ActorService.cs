using AutoMapper;
using SplititAssignment.Data;
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
            if (_actorRepository.RankExists(request.Rank, request.Source))
            {
                throw new DuplicateRankException(request.Name);
            }

            var actor = _mapper.Map<Actor>(request);
            if (!_actorRepository.AddActor(actor))
            {
                throw new();
            }

            var response = _mapper.Map<ActorResponse>(actor);
            return response;
        }

        public bool DeleteActor(string actorId)
        {
            var toRemove = _actorRepository.GetActor(actorId);

            if (toRemove == null)
            {
                throw new ValidationException();
            }

            return _actorRepository.DeleteActor(toRemove);
        }

        public ActorResponse GetActorDetails(string actorId)
        {
            var actor = _actorRepository.GetActor(actorId);

            if (actor == null)
            {
                throw new ValidationException();
            }

            return _mapper.Map<ActorResponse>(actor);
        }

        public IEnumerable<ActorSummary> GetActorsSummary(string provider, int? rankStart = null, int? rankEnd = null, int skip = 0, int take = 10)
        {
            var actors = _actorRepository.GetActors(provider, rankStart, rankEnd, skip, take);

            return _mapper.Map<IEnumerable<ActorSummary>>(actors);
        }

        public ActorResponse UpdateActor(string actorId, ActorRequest request)
        {
            var existingActor = GetActorDetails(actorId);

            if (existingActor == null)
            {
                throw new ValidationException();
            }

            if (_actorRepository.RankExists(request.Rank, request.Source))
            {
                throw new DuplicateRankException(request.Name);
            }

            var actor = _mapper.Map<Actor>(request);

            actor.Id = actorId;
            _actorRepository.Update(actor);
            return _mapper.Map<ActorResponse>(actor);
        }
    }
}
