
namespace PlotLineApp.Models
{
    public class Book
    {
        public string Title { get;set;}
        public string CoverImagePath { get; set; }

        public Book(string title,  string coverImagePath)
        {
            Title = title;
            CoverImagePath = coverImagePath;
        }
    }
}