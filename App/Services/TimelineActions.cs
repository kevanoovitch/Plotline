using PlotLineApp.Models;

namespace PlotlineApp.Services;

public static class TimelineActions
{
    // Centralizes removal logic and keeps callers (UI) trivial
    public static void RemoveFromMonth(MonthBook? month, Book? book)
    {
        if (month is null || book is null) return;
        month.RemoveBook(book);
    }
}