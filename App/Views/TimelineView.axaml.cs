using Avalonia.Controls;
using Avalonia.Input;
using PlotLineApp.Models;
using PlotLineApp.ViewModels;

namespace PlotLineApp.Views;

public partial class TimelineView : UserControl
{
    public TimelineView()
    {
        InitializeComponent();
    }

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains("book"))
        {
            e.DragEffects = DragDropEffects.Copy;
            e.Handled = true;
        }
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains("book") &&
            sender is Border border &&
            border.DataContext is MonthBook month)
           {
            var book = e.Data.Get("book") as Book;
            if (book is not null)
            {
                month.AssignBook(book);
            }
        }
    }
}
