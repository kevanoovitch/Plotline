using System;
using PlotLineApp.Models;

public class MonthBookBuilder
{
    private DateTime _month = new DateTime(2025, 1, 1);
    private readonly List<Book> _assigned = new();

    public MonthBookBuilder InMonth(int year, int month)
    {
        _month = new DateTime(year, month, 1);
        return this;
    }

    public MonthBookBuilder WithAssigned(params Book[] books)
    {
        _assigned.Clear();
        _assigned.AddRange(books);
        return this;
    }

    public MonthBookBuilder AddAssigned(Book book)
    {
        _assigned.Add(book);
        return this;
    }

    public MonthBook Build()
    {
        var m = new MonthBook { Month = _month };
        foreach (var b in _assigned)
            m.AssignBook(b);
        return m;
    }
}