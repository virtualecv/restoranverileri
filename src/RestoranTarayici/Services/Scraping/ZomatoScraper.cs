using Microsoft.Playwright;
using RestoranTarayici.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestoranTarayici.Services.Scraping
{
    public class ZomatoScraper : IScraper
    {
        private readonly Action<string> _log;
        public ZomatoScraper(Action<string> log) => _log = log;

        public async Task<List<RestaurantRecord>> ScrapeAsync(IBrowser browser, string il, string ilce, string kategori, bool includeReviews, CancellationToken token)
        {
            var list = new List<RestaurantRecord>();
            var page = await browser.NewPageAsync();

            string query = $"{il} {ilce} {kategori}";
            string url = $"https://www.zomato.com/{Uri.EscapeDataString(query)}";

            _log("Zomato sayfası açılıyor...");
            await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });
            await page.WaitForTimeoutAsync(3000);

            var cards = await page.QuerySelectorAllAsync("h4.sc-1hp8d8a-0");
            foreach (var card in cards)
            {
                token.ThrowIfCancellationRequested();
                string name = await card.InnerTextAsync();

                list.Add(new RestaurantRecord
                {
                    kaynak = "Zomato",
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