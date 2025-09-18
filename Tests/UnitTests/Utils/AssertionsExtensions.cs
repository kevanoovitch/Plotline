using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using FluentAssertions.Execution;
using PlotLineApp.Models;

namespace PlotLineApp.UnitTets.Utils;

public static class AssertionsExtensions
{
    public static void ShouldContainTitles(this IEnumerable<Book> books, params string[] expectedTitles)
    {
        var actual = books.Select(b => b.Title).ToArray();
        using (new AssertionScope())
        {
            actual.Should().BeEquivalentTo(expectedTitles, opt => opt.WithoutStrictOrdering());
        }
    }


    public static void ShouldContainTitlesInOrder(this IEnumerable<Book> books, params string[] expectedTitles)
    {
        var actual = books.Select(b => b.Title).ToArray();
        actual.Should().Equal(expectedTitles);
    }
       

}