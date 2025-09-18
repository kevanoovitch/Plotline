using PlotLineApp.Models;

namespace Plotline.UnitTests.Utils;

public static class TestData
{
    public static Book Dune() => new Book("Dune", "avares://PlotLineApp/Assets/dune.png");
    public static Book NineteenEightyFour() => new Book("1984", "avares://PlotLineApp/Assets/dune.png");

    public static Book Mistborn() => new Book("Mistborn", "avares://PlotLineApp/Assets/dune.png");

    public static IEnumerable<Book> DefaultBooks() => new[] {
        Dune(), NineteenEightyFour(), Mistborn()
    };
}