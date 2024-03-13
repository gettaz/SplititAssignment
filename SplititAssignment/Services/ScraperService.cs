using SplititAssignment.Interfaces;
using SplititAssignment.Models;

namespace SplititAssignment.Services
{
    public class ScraperService : IScraperService
    {
        private readonly IEnumerable<IScraper> _scrapers;
        private readonly IActorRepository _actorRepository;

        public ScraperService(IEnumerable<IScraper> scrapers, IActorRepository actorRepository)
        {
            _scrapers = scrapers;
            _actorRepository = actorRepository;
        }

        public async Task ScrapeActorsAsync()
        {
            foreach (var scraper in _scrapers)
            {
                var actors = await scraper.ScrapeActorsAsync();
                AddActorsToDatabase(actors);
            }
        }

        private void AddActorsToDatabase(List<Actor> actors)
        {
            _actorRepository.AddActors(actors);
        }
    }
}
