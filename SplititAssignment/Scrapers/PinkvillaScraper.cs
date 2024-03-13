using SplititAssignment.Models;
using System.Text.RegularExpressions;

namespace SplititAssignment.Scrapers
{
    public class PinkvillaScraper : ScraperBase
    {
        public PinkvillaScraper() : base("Pinkvilla", "https://www.pinkvilla.com/entertainment/hollywood/most-popular-actresses-1169988")
        {
        }

        public override List<Actor> ScrapeActors()
        {
            var listItems = _document.DocumentNode.SelectNodes("//h3/strong");
            List<Actor> actors = new List<Actor>();

            foreach (var node in listItems)
            {
                var detailsNode = node.ParentNode.SelectSingleNode("following-sibling::ul/following-sibling::p");
                var rankAndName = node.InnerText.Trim();
                var match = Regex.Match(rankAndName, @"^(\d+)\.\s*(.*)$");

                var actor = new Actor
                {
                    Name = match.Groups[2].Value,
                    Rank = int.Parse(match.Groups[1].Value),
                    Type = "Actress",
                    Details = detailsNode.InnerText.Trim(),
                    Source = _provider
                };
                actors.Add(actor);
            }

            return actors;
        }
    }
}
