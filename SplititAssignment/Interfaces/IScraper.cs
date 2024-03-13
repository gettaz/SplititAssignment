using SplititAssignment.Models;

namespace SplititAssignment.Interfaces
{
    public interface IScraper
    {
        Task<List<Actor>> ScrapeActorsAsync();
    }
}
