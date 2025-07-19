using System; 
using System.Windows.Input;
using System.Collections.ObjectModel;
using PlotLineApp.Models;
using PlotLineApp.Views;
using PlotLineApp.Services;

namespace PlotLineApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<MonthBook>? MonthlyBooks {get;}

    private readonly IAppCloser _appCloser;

    public ICommand GoToBooksViewCommand { get; }

    public event EventHandler? OpenBooksPopupRequested;

    public MainWindowViewModel(IAppCloser appCloser)
    {   

        

        _appCloser = appCloser;      

        ShutdownAppCommand = new RelayCommand(_ => _appCloser.Shutdown());

        GoToBooksViewCommand = new RelayCommand(() =>
        {

            Console.WriteLine("Requesting popup window..");
            OpenBooksPopupRequested?.Invoke(this, EventArgs.Empty);
            
        });

 
    }

    public ICommand ShutdownAppCommand {get;}
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

