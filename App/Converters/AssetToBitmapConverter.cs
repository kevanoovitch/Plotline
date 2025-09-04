using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

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
                    if (AssetLoader.Exists(uri))
                    {
                        using var stream = AssetLoader.Open(uri);
                        return new Bitmap(stream);
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

