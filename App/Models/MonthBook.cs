using System;
using System.Collections.ObjectModel;

namespace PlotLineApp.Models
{

    public class MonthBook
    {
        public DateTime Month { get; set; }

        // List of books dropped into this month 
        public ObservableCollection<Book> AssignedBooks { get; } = new();

        // Read-only display property
        public string MonthText => Month.ToString("MMMM");

        public void AssignBook(Book book)
        {
            if (!AssignedBooks.Contains(book))
            {
                AssignedBooks.Add(book);
            }
        }

    }
}