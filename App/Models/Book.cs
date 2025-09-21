using System;
using System.Collections.Generic;
using System.Linq;

namespace PlotLineApp.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string CoverImagePath { get; set; }
        public IReadOnlyList<string> Authors { get; set; }
        public int? PublishedYear { get; set; }
        public string? ExternalId { get; set; }
        public string? Isbn13 { get; set; }

        public Book(
            string title,
            string coverImagePath,
            IEnumerable<string>? authors = null,
            int? publishedYear = null,
            string? externalId = null,
            string? isbn13 = null)
        {
            Title = title;
            CoverImagePath = coverImagePath;
            Authors = authors?.ToArray() ?? Array.Empty<string>();
            PublishedYear = publishedYear;
            ExternalId = externalId;
            Isbn13 = isbn13;
        }
    }
}
