using RestoranTarayici.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace RestoranTarayici.Services.Exporters
{
    public static class JsonExporter
    {
        public static void Export(List<RestaurantRecord> data, string filePath)
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}