
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PlotLineApp.Models;
using App.API;
using AppBook = PlotLineApp.Models.Book;
using System.Threading; 
using System.Threading.Tasks;


namespace PlotLineApp.ViewModels;

public class BooksViewModel : ViewModelBase
{

    private CancellationTokenSource? _cts; 
    private bool _isLoading;
    public bool IsLoading { get => _isLoading;
    private set { _isLoading = value; 
    OnPropertyChanged();}}

    public ICommand ReturnToMainCommand {get;}

    public string? Error {get; private set;}
    
    
    

    private static AppBook Map(GoogleBook g) => new AppBook(
        title: g.Title,
        coverImagePath: g.CoverImageUrl ?? "avares://PlotLineApp/Assets/dune.png",
        authors: g.Authors,
        publishedYear : g.PublishedYear,
        externalId: g.ExternalId
    );

  

    private readonly IGoogleBooksInterface _catalog;

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
                _ = DebounceFetchAsync(_searchTerm);
	        }
	    }

        private async Task DebounceFetchAsync(string query)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var ct = _cts.Token;
            try 
            {
                IsLoading = true; 
                Error = null; OnPropertyChanged(nameof(Error));
                await Task.Delay(300,ct);

                var results = await _catalog.SearchAsync(query, 20 ,ct);
                AvailableBooks.Clear();
                foreach (var b in results.Select(Map))
                    AvailableBooks.Add(b);
                
                OnPropertyChanged(nameof(FilteredBooks));
                OnPropertyChanged(nameof(HasResults));
                OnPropertyChanged(nameof(HasNoResults));
            }
            catch (OperationCanceledException) {}
            catch (Exception)
            {
                Error = "Couln't fetch books. Check your connection.";
                OnPropertyChanged(nameof(Error));
            }
            finally
            {
                if (!ct.IsCancellationRequested) IsLoading = false;
            }
        }
    

    public IEnumerable<Book> FilteredBooks => AvailableBooks;
    public bool HasResults => AvailableBooks.Any();
    public bool HasNoResults => !HasResults;

    // FIXME: Remove old code
    
    /*
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
    */
    private readonly Action _onReturn;

    public BooksViewModel(Action onReturn, IGoogleBooksInterface catalog)
    {
        _onReturn = onReturn;
        _catalog = catalog;

        ReturnToMainCommand = new RelayCommand(_onReturn);

        AvailableBooks = new ObservableCollection<Book>();


        //FIXME: OLD code
        // Use Avalonia resource URIs for embedded assets (see csproj <AvaloniaResource Include="Assets/**" />)
        // Note: currently only dune.png exists in Assets; others point to dune.png as a placeholder.
        /*
        AvailableBooks = new ObservableCollection<Book>
        {
            new Book("Dune", "avares://PlotLineApp/Assets/dune.png"),
            new Book("1984", "avares://PlotLineApp/Assets/dune.png"),
            new Book("Mistborn", "avares://PlotLineApp/Assets/dune.png")
        };
        */
    }

    // Inherits ObservableObject.OnPropertyChanged from ViewModelBase
}
