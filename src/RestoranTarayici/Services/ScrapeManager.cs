using Microsoft.Playwright;
using RestoranTarayici.Models;
using RestoranTarayici.Services.Scraping;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestoranTarayici.Services
{
    public class ScrapeManager
    {
        private readonly Action<string> _log;

        public ScrapeManager(Action<string> log)
        {
            _log = log;
        }

        public async Task<List<RestaurantRecord>> RunAsync(
            List<string> sources,
            string il,
            string ilce,
            string kategori,
            bool includeReviews,
            CancellationToken token)
        {
            var results = new List<RestaurantRecord>();

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            foreach (var source in sources)
            {
                token.ThrowIfCancellationRequested();

                IScraper scraper = source switch
                {
                    "Google" => new GoogleScraper(_log),
                    "Tripadvisor" => new TripadvisorScraper(_log),
                    "Zomato" => new ZomatoScraper(_log),
                    "Foursquare" => new FoursquareScraper(_log),
                    _ => throw new NotSupportedException("Bilinmeyen kaynak: " + source)
                };

                _log($"{source} taraması başlıyor...");
                var data = await scraper.ScrapeAsync(browser, il, ilce, kategori, includeReviews, token);
                results.AddRange(data);
                _log($"{source} tamamlandı. {data.Count} kayıt.");
            }

            return results;
        }
    }
}