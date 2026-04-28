using System;
using System.IO;

namespace RestoranTarayici.Utils
{
    public static class FileNameHelper
    {
        public static string CreateBaseFileName(string il, string ilce, string kategori)
        {
            string baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RestoranCikti");
            Directory.CreateDirectory(baseDir);

            string safe = $"{il}_{ilce}_{kategori}_{DateTime.Now:yyyyMMdd_HHmmss}"
                .Replace(" ", "_")
                .Replace("ç", "c").Replace("ğ", "g").Replace("ı", "i")
                .Replace("ö", "o").Replace("ş", "s").Replace("ü", "u");

            return Path.Combine(baseDir, safe);
        }
    }
}