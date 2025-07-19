
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PlotLineApp.Models;

namespace PlotLineApp.ViewModels;

public class BooksViewModel : ViewModelBase
{
    public ObservableCollection<Book> AvailableBooks{get;}

    public ICommand ReturnToMainCommand { get; }

    private readonly Action _onReturn; 

    public BooksViewModel(Action onReturn)
    {
        _onReturn = onReturn;

        ReturnToMainCommand = new RelayCommand(_onReturn);

        // TODO do not hard code this like this
        AvailableBooks = new ObservableCollection<Book>
        {
            new Book("Dune", "/Assets/dune.png"),
            new Book("1984", "/Assets/1984.png"),
            new Book("Mistborn", "/Assets/mistborn.png")
        };
    }
}