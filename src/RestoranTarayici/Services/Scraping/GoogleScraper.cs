using Microsoft.Playwright;
using RestoranTarayici.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestoranTarayici.Services.Scraping
{
    public class GoogleScraper : IScraper
    {
        private readonly Action<string> _log;
        public GoogleScraper(Action<string> log) => _log = log;

        public async Task<List<RestaurantRecord>> ScrapeAsync(IBrowser browser, string il, string ilce, string kategori, bool includeReviews, CancellationToken token)
        {
            var list = new List<RestaurantRecord>();
            var page = await browser.NewPageAsync();

            string query = $"{il} {ilce} {kategori}";
            string url = $"https://www.google.com/maps/search/{Uri.EscapeDataString(query)}";

            _log("Google Maps sayfası açılıyor...");
            await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded });

            await page.WaitForTimeoutAsync(3000);

            var cards = await page.QuerySelectorAllAsync("div.Nv2PK");
            foreach (var card in cards)
            {
                token.ThrowIfCancellationRequested();

                string name = await card.QuerySelectorAsync("div.qBF1Pd") is IElementHandle nameEl
                    ? await nameEl.InnerTextAsync()
                    : "";

                string rating = await card.QuerySelectorAsync("span.MW4etd") is IElementHandle ratingEl
                    ? await ratingEl.InnerTextAsync()
                    : "";

                string reviews = await card.QuerySelectorAsync("span.UY7F9") is IElementHandle reviewsEl
                    ? await reviewsEl.InnerTextAsync()
                    : "";

                list.Add(new RestaurantRecord
                {
                    kaynak = "Google",
                    il = il,
                    ilce = ilce,
                    kategori = kategori,
                    restoran_adi = name,
                    puan = rating,
                    yorum_sayisi = reviews,
                    cekim_tarihi = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    url = url
                });
            }

            await page.CloseAsync();
            return list;
        }
    }
}