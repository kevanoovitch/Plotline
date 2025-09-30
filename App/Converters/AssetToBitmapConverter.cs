using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System.Net.Http;

namespace PlotLineApp.Converters
{
    public class AssetToBitmapConverter : IValueConverter
    {

        private static readonly HttpClient Http = new()
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        static AssetToBitmapConverter()
        {
            Http.DefaultRequestHeaders.UserAgent.ParseAdd("PlotLineApp/1.0");
        }


        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string raw || string.IsNullOrWhiteSpace(raw))
                return LoadPlaceholder();

            try
            {
                // If it's already an absolute URI, try to load via assets first
                if (Uri.TryCreate(raw, UriKind.Absolute, out var uri))
                {
                    if (uri.Scheme is "http" or "https")
                    {
                        // Normalize https
                        if (uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                            uri = new UriBuilder(uri) { Scheme = "https", Port = -1 }.Uri;

                        using var resp = Http.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();

                        var bytes = resp.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                            
                        



                        if (!resp.IsSuccessStatusCode)
                            return LoadPlaceholder();

                        using var netStream = resp.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                        return new Bitmap(netStream);
                    }

                    if (uri.Scheme is "avares" && AssetLoader.Exists(uri))
                    {
                        using var stream = AssetLoader.Open(uri);
                        return new Bitmap(stream);
                    }
                }
                else
                {
                    // Treat as app-relative asset path (e.g., "Assets/dune.png")
                    var appName = typeof(AssetToBitmapConverter).Assembly.GetName().Name;
                    var assetUri = new Uri($"avares://{appName}/{raw.TrimStart('/')}");
                    if (AssetLoader.Exists(assetUri))
                    {
                        using var stream = AssetLoader.Open(assetUri);
                        return new Bitmap(stream);
                    }
                }

                // Fallback to file system path
                if (System.IO.File.Exists(raw))
                {
                    return new Bitmap(raw);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Image download failed: {ex.Message}");
                return LoadPlaceholder();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AssetToBitmapConverter error: {ex}");
            }
            return LoadPlaceholder();
        }

        private static Bitmap? LoadPlaceholder()
        {
            var appName = typeof(AssetToBitmapConverter).Assembly.GetName().Name;
            var placeholder = new Uri($"avares://{appName}/Assets/GenericCover.png");
            if (!AssetLoader.Exists(placeholder)) return null;

            using var stream = AssetLoader.Open(placeholder);
            return new Bitmap(stream);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
