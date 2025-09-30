using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace App.API;

public sealed class GoogleBooksInterface : IGoogleBooksInterface
{
    private readonly HttpClient _http = new() { BaseAddress = new Uri("https://www.googleapis.com/") };
    private readonly string? _apiKey = ReadApiKey();

    public async Task<IReadOnlyList<GoogleBook>> SearchAsync(string query, int limit = 20, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Array.Empty<GoogleBook>();

        limit = Math.Clamp(limit, 1, 40);

        var q = Uri.EscapeDataString(query.Trim());
        var url = $"books/v1/volumes?q={q}&maxResults={limit}";
        if (!string.IsNullOrWhiteSpace(_apiKey)) url += $"&key={_apiKey}";

        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        using var resp = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);

        if (!resp.IsSuccessStatusCode)
            return Array.Empty<GoogleBook>();

        await using var stream = await resp.Content.ReadAsStreamAsync(ct);
        var payload = await JsonSerializer.DeserializeAsync<GoogleVolumesResponse>(
            stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            ct);

        if (payload?.Items is null || payload.Items.Length == 0)
            return Array.Empty<GoogleBook>();

        var list = new List<GoogleBook>(payload.Items.Length);
        foreach (var item in payload.Items)
        {
            var info = item?.VolumeInfo;
            if (info is null) continue;

            var title = info.Title?.Trim();
            if (string.IsNullOrEmpty(title)) continue;

            var cover = info.ImageLinks?.Thumbnail ?? info.ImageLinks?.SmallThumbnail;
            var year = ParseYear(info.PublishedDate);
            var authors = (IReadOnlyList<string>)(info.Authors ?? Array.Empty<string>());


            list.Add(new GoogleBook
            {
                Title = title,
                CoverImageUrl = cover,
                Authors = authors,
                PublishedYear = year,
                ExternalId = item!.Id
            });
        }

        return list;
    }

    private static int? ParseYear(string? publishedDate)
    {
        if (string.IsNullOrWhiteSpace(publishedDate)) return null;
        // Try first 4 digits anywhere in the string
        for (int i = 0; i + 3 < publishedDate.Length; i++)
        {
            if (int.TryParse(publishedDate.AsSpan(i, 4), out var y) && y >= 1400 && y <= DateTime.UtcNow.Year + 1)
                return y;
        }
        return null;
    }

    private static string? ReadApiKey() => Environment.GetEnvironmentVariable("GOOGLE_BOOKS_API_KEY");
}

file sealed class GoogleVolumesResponse
{
    public GoogleVolume[]? Items { get; set; }
}

file sealed class GoogleVolume
{
    public string? Id { get; set; }
    public GoogleVolumeInfo? VolumeInfo { get; set; }
}

file sealed class GoogleVolumeInfo
{
    public string? Title { get; set; }
    public string[]? Authors { get; set; }
    public string? PublishedDate { get; set; }
    public GoogleImageLinks? ImageLinks { get; set; }
}

file sealed class GoogleImageLinks
{
    public string? Thumbnail { get; set; }
    public string? SmallThumbnail { get; set; }
}
