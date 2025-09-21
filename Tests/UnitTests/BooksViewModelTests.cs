using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using PlotLineApp.ViewModels;
using Xunit;
using App.API;

namespace Plotline.UnitTests;

public class BooksViewModelTests
{
    private sealed class FakeCatalog : IGoogleBooksInterface
    {
        private readonly IReadOnlyList<GoogleBook> _items;
        public FakeCatalog()
        {
            _items = new[]
            {
                new GoogleBook { Title = "Dune", CoverImageUrl = null, Authors = new[] { "Frank Herbert" }, PublishedYear = 1965, ExternalId = "1" },
                new GoogleBook { Title = "1984", CoverImageUrl = null, Authors = new[] { "George Orwell" }, PublishedYear = 1949, ExternalId = "2" },
                new GoogleBook { Title = "Mistborn", CoverImageUrl = null, Authors = new[] { "Brandon Sanderson" }, PublishedYear = 2006, ExternalId = "3" }
            };
        }
        public Task<IReadOnlyList<GoogleBook>> SearchAsync(string query, int limit = 20, CancellationToken ct = default)
            => Task.FromResult(_items);
    }

    [Fact]
    public async Task InitialState_ShowsAllBooks_AndFlagsTrue()
    {
        var vm = new BooksViewModel(() => { }, new FakeCatalog());
        vm.SearchTerm = "seed"; // trigger fetch
        await Task.Delay(400);

        var all = vm.FilteredBooks.ToList();
        all.Should().NotBeEmpty();
        vm.HasResults.Should().BeTrue();
        vm.HasNoResults.Should().BeFalse();
    }

    [Theory]
    [InlineData("dun")] //Partial match
    [InlineData("Dune")] //Case sensitive
    [InlineData("  dune  ")] //Trimmed
    public async Task FilteredBooks_FiltersByTitle(string query)
    {
        var vm = new BooksViewModel(() => { }, new FakeCatalog());
        vm.SearchTerm = query; // triggers fetch
        await Task.Delay(400);

        vm.FilteredBooks.Should().OnlyContain(b => b.Title.Contains("Dune", System.StringComparison.OrdinalIgnoreCase)).And.HaveCount(1);

        vm.HasResults.Should().BeTrue();
        vm.HasNoResults.Should().BeFalse();
    }

    [Fact]
    public async Task NoMatches_SetsHasNoResults_AndEmptyFiltered()
    {
        var vm = new BooksViewModel(() => { }, new FakeCatalog());

        vm.SearchTerm = "zzzzzz";
        await Task.Delay(400);

        vm.FilteredBooks.Should().BeEmpty();
        vm.HasResults.Should().BeFalse();
        vm.HasNoResults.Should().BeTrue();
    }

    [Fact]
    public async Task ClearingSearch_RestoresAllAndFlags()
    {
        var vm = new BooksViewModel(() => { }, new FakeCatalog());

        vm.SearchTerm = "seed";
        await Task.Delay(400);
        var all = vm.FilteredBooks.ToList();
        vm.SearchTerm = "zzz"; // Force no results
        await Task.Delay(400);
        vm.SearchTerm = string.Empty; // clear
        await Task.Delay(400);

        vm.FilteredBooks.Should().BeEquivalentTo(all);
        vm.HasResults.Should().BeTrue();
        vm.HasNoResults.Should().BeFalse();
    }
}
