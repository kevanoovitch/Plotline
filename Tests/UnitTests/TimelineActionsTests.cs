using System.Linq.Expressions;
using FluentAssertions;
using PlotlineApp.Services;
using PlotLineApp.Models;
using PlotLineApp.Services;

namespace Plotline.UnitTests;

public class TimelineActionsTests
{
    [Fact]
    public void RemoveFromMonth_Remove_WhenPresent()
    {
        var month = new MonthBook();
        var dune = new Book("Dune", "x");
        month.AssignBook(dune);

        TimelineActions.RemoveFromMonth(month, dune);

        month.AssignedBooks.Should().BeEmpty();
    }

    [Fact]
    public void RemoveFromMonth_NoOp_WhenNotPresent()
    {
        var month = new MonthBook();
        var dune = new Book("Dune", "x");

        TimelineActions.RemoveFromMonth(month, dune);

        month.AssignedBooks.Should().BeEmpty();
    }

    [Fact]
    public void RemoveFromMonth_IsSafe_OnNullMonth()
    {
        var dune = new Book("Dune", "x");
        TimelineActions.RemoveFromMonth(null, dune);
        // no exception expected
        true.Should().BeTrue();
    }

    [Fact]
    public void RemoveFromMonth_IsSafe_OnNullBook()
    {
        var month = new MonthBook();
        TimelineActions.RemoveFromMonth(month, null);
        month.AssignedBooks.Should().BeEmpty();
    }
}