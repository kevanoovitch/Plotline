using System.Linq;
using FluentAssertions;
using PlotLineApp.ViewModels;
using Xunit;

namespace Plotline.UnitTests;

public class BooksViewModelTests
{
    [Fact]
    public void InitialState_ShowsAllBooks_AndFlagsTrue()
    {
        var vm = new BooksViewModel(() => { });

        var all = vm.FilteredBooks.ToList();
        all.Should().NotBeEmpty();
        vm.HasResults.Should().BeTrue();
        vm.HasNoResults.Should().BeFalse();
    }

    [Theory]
    [InlineData("dun")] //Partial match
    [InlineData("Dune")] //Case sensitive
    [InlineData("  dune  ")] //Trimmed
    public void FilteredBooks_FiltersByTitle(string query)
    {
        var vm = new BooksViewModel(() => { });
        vm.SearchTerm = query;

        vm.FilteredBooks.Should().OnlyContain(b => b.Title.Contains("Dune", System.StringComparison.OrdinalIgnoreCase)).And.HaveCount(1);

        vm.HasResults.Should().BeTrue();
        vm.HasNoResults.Should().BeFalse();
    }

    [Fact]
    public void NoMatches_SetsHasNoResults_AndEmptyFiltered()
    {
        var vm = new BooksViewModel(() => { });

        vm.SearchTerm = "zzzzzz";

        vm.FilteredBooks.Should().BeEmpty();
        vm.HasResults.Should().BeFalse();
        vm.HasNoResults.Should().BeTrue();
    }

    [Fact]

    public void ClearingSearch_RestoresAllAndFlags()
    {
        var vm = new BooksViewModel(() => { });

        var all = vm.FilteredBooks.ToList();
        vm.SearchTerm = "zzz"; // Force no results
        vm.SearchTerm = string.Empty; // clear

        vm.FilteredBooks.Should().BeEquivalentTo(all);
        vm.HasResults.Should().BeTrue();
        vm.HasNoResults.Should().BeFalse();
    }
}