using System; 
using System.Collections.ObjectModel;
using PlotLineApp.Models;

namespace PlotLineApp.ViewModels; 

public class TimelineViewModel : ViewModelBase 
{
    public ObservableCollection<MonthBook> MonthlyBooks { get;}

    public TimelineViewModel()
    {
        MonthlyBooks = new ObservableCollection<MonthBook>();
        int currentYear = DateTime.Now.Year;

        for (int month = 1; month <= 12; month++)
        {
            MonthlyBooks.Add(new MonthBook
            {
                Month = new DateTime(currentYear,month,1),
                BookTitle = "A default title" // TODO Remove placeholder string
            });
        }

    }
}