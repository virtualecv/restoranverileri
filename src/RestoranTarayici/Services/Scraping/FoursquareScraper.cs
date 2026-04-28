using Microsoft.Playwright;
using RestoranTarayici.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestoranTarayici.Services.Scraping
{
    public class FoursquareScraper : IScraper
    {
        private readonly Action<string> _log;
        public FoursquareScraper(Action<string> log) => _log = log;

        public async Task<List<RestaurantRecord>> ScrapeAsync(IBrowser browser, string il, string ilce, string kategori, bool includeReviews, CancellationToken token)
        {
            var list = new List<RestaurantRecord>();
            var page = await browser.NewPageAsync();

            string query = $"{il} {ilce} {kategori}";
            string url = $"https://foursquare.com/explore?mode=url&near={Uri.EscapeDataString(il + " " + ilce)}&q={Uri.EscapeDataString(kategori)}";

            _log("Foursquare sayfası açılıyor...");
            await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
            await page.WaitForTimeoutAsync(3000);

            var cards = await page.QuerySelectorAllAsync("div.venueName");
            foreach (var card in cards)
            {
                token.ThrowIfCancellationRequested();
                string name = await card.InnerTextAsync();

                list.Add(new RestaurantRecord
                {
                    kaynak = "Foursquare",
                    il = il,
                    ilce = ilce,
                    kategori = kategori,
                    restoran_adi = name,
                    cekim_tarihi = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    url = url
                });
            }

            await page.CloseAsync();
            return list;
        }
    }
}