using SplititAssignment.Interfaces;
using SplititAssignment.Models;
using HtmlAgilityPack;
using System.Xml.Linq;
using System.Collections.Generic;

namespace SplititAssignment.Scrapers
{
    public class IMDbScraper : ScraperBase
    {
        public IMDbScraper() : base("IMDb","https://www.imdb.com/list/ls005759317/")
        {
        }

        public override List<Actor> ScrapeActors()
        {
            var nodes = _document.DocumentNode.SelectNodes("//div[contains(@class, 'lister-item mode-detail')]");
            List<Actor> actors = new List<Actor>();

            foreach (var node in nodes)
            {
                var rankNode = node.SelectSingleNode(".//span[@class='lister-item-index unbold text-primary']");
                var actor = new Actor
                {
                    Name = node.SelectSingleNode(".//h3/a").InnerText.Trim(),
                    Rank = int.Parse(rankNode.InnerText.Trim().TrimEnd('.')),
                    Type = node.SelectSingleNode(".//p").InnerText.Trim().Split(new[] { '|' }, 2)[0].Trim(),
                    Details = node.SelectSingleNode(".//p[last()]").InnerText.Trim(),
                    Source = _provider
                };
                actors.Add(actor);
            }

            return actors;
        }

    }

}
