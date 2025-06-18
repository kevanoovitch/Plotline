using System; 
using System.Collections.ObjectModel;
using Plotline.Models;

namespace PlotLineApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<MonthBook> MonthlyBooks {get;}

    public MainWindowViewModel()
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
