
namespace PlotLineApp.Models
{
    public class Book
    {
        public string Title {get;set;}
        public string ImagePath {get;set;}

        public Book(string title, string image)
        {
            Title = title;
            ImagePath = image; 
        }
    }
}