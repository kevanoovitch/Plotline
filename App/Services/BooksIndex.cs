using System.Collections.Generic;
using System.Linq;
using PlotLineApp.Models;

namespace PlotLineApp.Services
{
    public static class BooksIndex
    {
        public static IReadOnlyList<Book> Available { get; set; } = new List<Book>();

        public static Book? FindByTitle(string title) => Available.FirstOrDefault(b => b.Title == title);
    }
}
