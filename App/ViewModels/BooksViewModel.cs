
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

    private const string DefaultCover = "avares://PlotLineApp/Assets/GenericCover.png";


    private CancellationTokenSource? _cts; 
    private bool _isLoading;
    public bool IsLoading { get => _isLoading;
    private set { _isLoading = value; 
    OnPropertyChanged();}}

    public ICommand ReturnToMainCommand {get;}

    public string? Error {get; private set;}


    private static AppBook Map(GoogleBook g)
    {
        var cover = g.CoverImageUrl;

        if (!string.IsNullOrWhiteSpace(cover))
        {
            if (cover.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                cover = "https://" + cover[7..];

        }
        else
        {
            cover = DefaultCover;
        }

        return new AppBook(
            title: g.Title,
            coverImagePath: cover,
            authors: g.Authors,
            publishedYear: g.PublishedYear,
            externalId: g.ExternalId
        );

    }


  

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

   
    private readonly Action _onReturn;

    public BooksViewModel(Action onReturn, IGoogleBooksInterface catalog)
    {
        _onReturn = onReturn;
        _catalog = catalog;

        ReturnToMainCommand = new RelayCommand(_onReturn);

        AvailableBooks = new ObservableCollection<Book>();


    }

    // Inherits ObservableObject.OnPropertyChanged from ViewModelBase
}
