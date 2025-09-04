
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PlotLineApp.Models;

namespace PlotLineApp.ViewModels;

public class BooksViewModel : ViewModelBase
{
    public ObservableCollection<Book> AvailableBooks { get; }

	    private string _searchTerm = string.Empty;
	    public string SearchTerm
	    {
	        get => _searchTerm;
	        set
	        {
	            if (_searchTerm == value) return;
	            _searchTerm = value;
	            OnPropertyChanged();
	            OnPropertyChanged(nameof(FilteredBooks));
	            OnPropertyChanged(nameof(HasResults));
	            OnPropertyChanged(nameof(HasNoResults));
	        }
	    }

    public IEnumerable<Book> FilteredBooks
    {
        get
        {
            var q = SearchTerm?.Trim();
            if (string.IsNullOrEmpty(q)) return AvailableBooks;
            return AvailableBooks.Where(b => (!string.IsNullOrEmpty(b.Title) &&
            b.Title.Contains(q, StringComparison.OrdinalIgnoreCase))
            );
        }
    }

	public bool HasResults => FilteredBooks.Any();
	public bool HasNoResults => !HasResults;
	public ICommand ReturnToMainCommand { get; }

    private readonly Action _onReturn;

    public BooksViewModel(Action onReturn)
    {
        _onReturn = onReturn;

        ReturnToMainCommand = new RelayCommand(_onReturn);

        // TODO do not hard code this like this
        // Use Avalonia resource URIs for embedded assets (see csproj <AvaloniaResource Include="Assets/**" />)
        // Note: currently only dune.png exists in Assets; others point to dune.png as a placeholder.
        AvailableBooks = new ObservableCollection<Book>
        {
            new Book("Dune", "avares://PlotLineApp/Assets/dune.png"),
            new Book("1984", "avares://PlotLineApp/Assets/dune.png"),
            new Book("Mistborn", "avares://PlotLineApp/Assets/dune.png")
        };
    }

    // Inherits ObservableObject.OnPropertyChanged from ViewModelBase
}
