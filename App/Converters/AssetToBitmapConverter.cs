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
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string s || string.IsNullOrWhiteSpace(s))
                return null;

            try
            {
                // If it's already an absolute URI, try to load via assets first
                if (Uri.TryCreate(s, UriKind.Absolute, out var uri))
                {
                    // 1) Avalonia asset URI (avares://...)
                    if (AssetLoader.Exists(uri))
                    {
                        using var stream = AssetLoader.Open(uri);
                        return new Bitmap(stream);
                    }

                    // 2) Remote HTTP(S) image
                    if (uri.Scheme is "http" or "https")
                    {
                        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
                        using var resp = http.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult();
                        if (resp.IsSuccessStatusCode)
                        {
                            using var netStream = resp.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
                            return new Bitmap(netStream);
                        }
                    }
                }
                else
                {
                    // Treat as app-relative asset path (e.g., "Assets/dune.png")
                    var appName = typeof(AssetToBitmapConverter).Assembly.GetName().Name;
                    var assetUri = new Uri($"avares://{appName}/{s.TrimStart('/')}");
                    if (AssetLoader.Exists(assetUri))
                    {
                        using var stream = AssetLoader.Open(assetUri);
                        return new Bitmap(stream);
                    }
                }

                // Fallback to file system path
                if (System.IO.File.Exists(s))
                {
                    return new Bitmap(s);
                }
            }
            catch
            {
                // Ignore and return null, Image will render empty
            }
            return null;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
