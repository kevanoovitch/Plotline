using System; 
using System.Windows.Input;
using System.Collections.ObjectModel;
using Plotline.Models;

namespace PlotLineApp.ViewModels;

public class RelayCommand : ICommand 
{
    // A relay cmd to open the bookView

    private readonly Action _execute; 
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? _canExecute = null)
    {
        _execute = execute;
        _canExecute = _canExecute;
    }

    public event EventHandler? CanExecuteChanged; 

    public bool CanExecute(Object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);    
    }
}


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

        GoToBooksViewCommand = new RelayCommand(() => 
        {
            CurrentView = new BooksView();
        });
    }
}


