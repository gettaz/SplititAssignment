using HtmlAgilityPack;
using SplititAssignment.Interfaces;
using SplititAssignment.Models;

namespace SplititAssignment.Scrapers
{
    public abstract class ScraperBase : IScraper
    {
        protected HtmlDocument _document;
        protected string _provider;
        protected string _url;
        protected HtmlWeb _web;

        public ScraperBase(string provider, string url)
        {
            _web = new HtmlWeb();
            _url = url;
            _provider = provider;
        }

        protected async Task LoadDocumentAsync()
        {
            _document = await _web.LoadFromWebAsync(_url);
        }

        public abstract List<Actor> ScrapeActors();

        public async Task<List<Actor>> ScrapeActorsAsync()
        {
            await LoadDocumentAsync();
            return ScrapeActors();
        }
    }
}

