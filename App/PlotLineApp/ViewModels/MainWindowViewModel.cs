using System; 
using System.Windows.Input;
using System.Collections.ObjectModel;
using PlotLineApp.Models;
using PlotLineApp.Views;

namespace PlotLineApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<MonthBook> MonthlyBooks {get;}

    public ICommand GoToBooksViewCommand { get; }

    private object? _currentView;

    public object? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }
    

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
            try 
            {
                Console.WriteLine("Switching to BooksView..");
                CurrentView = new BooksViewModel(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed to switch to BooksView: " + ex.Message);
            }
        });

        CurrentView = new YourDefaultViewModel(); 
    }
}


public class RelayCommand : ICommand 
{
    // A relay cmd to open the bookView

    private readonly Action<object?> _execute; 
    private readonly Func<object?, bool>? _canExecute;

    // first constructor for cmds with params
    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        // _execute = execute ?? throw new ArgumentNullException(nameof(execute)); //this line is the clean way of doing the rest
        if (execute != null)
            _execute = execute;
        else
            throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    // constructor that overloads for paramless usage

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = _ => execute();
        _canExecute = canExecute == null ? null : (_ => canExecute());
    }

    public event EventHandler? CanExecuteChanged; 

    public bool CanExecute(object? parameter)
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