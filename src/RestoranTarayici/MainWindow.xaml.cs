using RestoranTarayici.Services;
using RestoranTarayici.Services.Exporters;
using RestoranTarayici.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace RestoranTarayici
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource? _cts;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            BtnStart.IsEnabled = false;
            BtnStop.IsEnabled = true;
            TxtLog.Clear();

            _cts = new CancellationTokenSource();

            string il = TxtIl.Text.Trim();
            string ilce = TxtIlce.Text.Trim();
            string kategori = (CmbKategori.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Restoran";
            bool includeReviews = ChkYorumlar.IsChecked == true;

            var sources = new List<string>();
            if (ChkGoogle.IsChecked == true) sources.Add("Google");
            if (ChkTripadvisor.IsChecked == true) sources.Add("Tripadvisor");
            if (ChkZomato.IsChecked == true) sources.Add("Zomato");
            if (ChkFoursquare.IsChecked == true) sources.Add("Foursquare");

            if (sources.Count == 0)
            {
                Log("Lütfen en az bir kaynak seçin.");
                BtnStart.IsEnabled = true;
                BtnStop.IsEnabled = false;
                return;
            }

            var manager = new ScrapeManager(Log);

            try
            {
                var results = await manager.RunAsync(sources, il, ilce, kategori, includeReviews, _cts.Token);

                string baseFile = FileNameHelper.CreateBaseFileName(il, ilce, kategori);

                if (ChkJson.IsChecked == true)
                    JsonExporter.Export(results, $"{baseFile}.json");

                if (ChkCsv.IsChecked == true)
                    CsvExporter.Export(results, $"{baseFile}.csv");

                if (ChkXlsx.IsChecked == true)
                    XlsxExporter.Export(results, $"{baseFile}.xlsx");

                Log($"Tamamlandı. Kayıt dosyaları: {baseFile}.*");
            }
            catch (OperationCanceledException)
            {
                Log("İşlem durduruldu.");
            }
            catch (Exception ex)
            {
                Log("Hata: " + ex.Message);
            }
            finally
            {
                BtnStart.IsEnabled = true;
                BtnStop.IsEnabled = false;
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            _cts?.Cancel();
            BtnStop.IsEnabled = false;
            Log("Durdurma isteği gönderildi...");
        }

        private void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                TxtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
                TxtLog.ScrollToEnd();
            });
        }
    }
}