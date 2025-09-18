using FluentAssertions;
using PlotLineApp.Models;
using Xunit;
using Plotline.UnitTests.Builders;

namespace Plotline.UnitTests;

public class MonthBookTest
{
    [Fact]
    public void AssignBook_PreventsDuplicates()
    {
        var month = new MonthBook();
        var dune = new BookBuilder().WithTitle("Dune").Build();

        month.AssignBook(dune);
        month.AssignBook(dune);

        month.AssignedBooks.Should().HaveCount(1);
        month.AssignedBooks.Should().Contain(dune);
    }

    [Fact]
    public void RemoveBook_IsSafe_OnNull()
    {
        var month = new MonthBook();

        month.RemoveBook(null!);

        month.AssignedBooks.Should().BeEmpty();
    }
}