using Microsoft.EntityFrameworkCore;
using SplititAssignment.Data;
using SplititAssignment.Interfaces;
using SplititAssignment.Models;

namespace SplititAssignment.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly DataContext _dataContext;

        public ActorRepository(DataContext context)
        {
            _dataContext = context;
        }

        public void AddActor(Actor actor)
        {
            _dataContext.Actors.Add(actor);
            Save();
        }

        public void AddActors(IEnumerable<Actor> actors)
        {
            _dataContext.Actors.AddRange(actors);
            Save();
        }

        public void DeleteActor(Actor actor)
        {
            _dataContext.Remove(actor);
            Save();
        }

        public Actor GetActor(string actorId)
        {
            return _dataContext.Actors.FirstOrDefault(actor => actor.Id == actorId);
        }

        public IEnumerable<Actor> GetActors(string provider, int? rankStart = null, int? rankEnd = null, int skip = 0, int take = 10)
        {
            var query = _dataContext.Actors.AsQueryable();

            if (!string.IsNullOrEmpty(provider))
            {
                query = query.Where(a => a.Source == provider);
            }

            if (rankStart.HasValue)
            {
                query = query.Where(a => a.Rank >= rankStart.Value);
            }

            if (rankEnd.HasValue)
            {
                query = query.Where(a => a.Rank <= rankEnd.Value);
            }

            query = query.Skip(skip).Take(take);

            return query.ToList();
        }

        public bool NameExists(string name, string provider)
        {
            return _dataContext.Actors.Any(actor => actor.Source == provider && actor.Name == name);
        }

        public string GetRankId(int rank, string provider)
        {
            return _dataContext.Actors.Where(actor => actor.Source == provider && actor.Rank == rank).Select(ac => ac.Id).First();
        }

        public void Update(Actor actor)
        {
            _dataContext.Update(actor);
            Save();
        }

        private void Save()
        {
            _dataContext.SaveChanges();
        }
    }
}
