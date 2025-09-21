using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace App.API;

public interface IGoogleBooksInterface
{
    Task<IReadOnlyList<GoogleBook>> SearchAsync(string query, int limit = 20, CancellationToken ct = default);
}

public sealed class GoogleBook
{
    public required string Title { get; init; }
    public string? CoverImageUrl { get; init; }
    public IReadOnlyList<string> Authors { get; init; } = Array.Empty<string>();
    public int? PublishedYear { get; init; }
    public string? ExternalId { get; init; }
}
