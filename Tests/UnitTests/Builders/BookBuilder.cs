using PlotLineApp.Models;

namespace Plotline.UnitTests.Builders;

public class BookBuilder
{
    private string _title = "Dune";

    private string _cover = "avares://PlotLineApp/Assets/dune.png";

    public BookBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public BookBuilder WithCover(string coverPath)
    {
        _cover = coverPath;
        return this;
    }

    public Book Build() => new Book(_title, _cover);
}