using Microsoft.Playwright;
using RestoranTarayici.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestoranTarayici.Services.Scraping
{
    public interface IScraper
    {
        Task<List<RestaurantRecord>> ScrapeAsync(IBrowser browser, string il, string ilce, string kategori, bool includeReviews, CancellationToken token);
    }
}