using RestoranTarayici.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RestoranTarayici.Services.Exporters
{
    public static class CsvExporter
    {
        public static void Export(List<RestaurantRecord> data, string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("kaynak,il,ilce,kategori,restoran_adi,adres,telefon,puan,yorum_sayisi,yorumlar,url,cekim_tarihi");

            foreach (var r in data)
            {
                sb.AppendLine($"{Esc(r.kaynak)},{Esc(r.il)},{Esc(r.ilce)},{Esc(r.kategori)},{Esc(r.restoran_adi)},{Esc(r.adres)},{Esc(r.telefon)},{Esc(r.puan)},{Esc(r.yorum_sayisi)},{Esc(r.yorumlar)},{Esc(r.url)},{Esc(r.cekim_tarihi)}");
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        private static string Esc(string? input)
        {
            if (input == null) return "";
            var s = input.Replace("\"", "\"\"");
            if (s.Contains(",") || s.Contains("\n") || s.Contains("\r"))
                s = $"\"{s}\"";
            return s;
        }
    }
}