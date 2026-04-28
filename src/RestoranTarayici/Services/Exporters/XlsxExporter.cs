using ClosedXML.Excel;
using RestoranTarayici.Models;
using System.Collections.Generic;

namespace RestoranTarayici.Services.Exporters
{
    public static class XlsxExporter
    {
        public static void Export(List<RestaurantRecord> data, string filePath)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Restoranlar");

            ws.Cell(1, 1).Value = "kaynak";
            ws.Cell(1, 2).Value = "il";
            ws.Cell(1, 3).Value = "ilce";
            ws.Cell(1, 4).Value = "kategori";
            ws.Cell(1, 5).Value = "restoran_adi";
            ws.Cell(1, 6).Value = "adres";
            ws.Cell(1, 7).Value = "telefon";
            ws.Cell(1, 8).Value = "puan";
            ws.Cell(1, 9).Value = "yorum_sayisi";
            ws.Cell(1, 10).Value = "yorumlar";
            ws.Cell(1, 11).Value = "url";
            ws.Cell(1, 12).Value = "cekim_tarihi";

            int row = 2;
            foreach (var r in data)
            {
                ws.Cell(row, 1).Value = r.kaynak;
                ws.Cell(row, 2).Value = r.il;
                ws.Cell(row, 3).Value = r.ilce;
                ws.Cell(row, 4).Value = r.kategori;
                ws.Cell(row, 5).Value = r.restoran_adi;
                ws.Cell(row, 6).Value = r.adres;
                ws.Cell(row, 7).Value = r.telefon;
                ws.Cell(row, 8).Value = r.puan;
                ws.Cell(row, 9).Value = r.yorum_sayisi;
                ws.Cell(row, 10).Value = r.yorumlar;
                ws.Cell(row, 11).Value = r.url;
                ws.Cell(row, 12).Value = r.cekim_tarihi;
                row++;
            }

            ws.Columns().AdjustToContents();
            wb.SaveAs(filePath);
        }
    }
}