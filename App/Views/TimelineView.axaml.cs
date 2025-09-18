using Avalonia.Controls;
using Avalonia.Input;
using PlotLineApp.Models;
using PlotLineApp.ViewModels;
using PlotLineApp.Services;
using Plotline.DnD;
using System;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using PlotlineApp.Services;

namespace PlotLineApp.Views;

public partial class TimelineView : UserControl
{
    public TimelineView()
    {
        InitializeComponent();
    }

    private void OnAddBookClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.DataContext is not MonthBook month)
            return;

        var popup = new BooksWindow
        {
            DataContext = new ViewModels.BooksViewModel(() => { })
        };

        popup.BookChosen += Book =>
        {
            month.AssignBook(Book);
            popup.Close();
        };

        if (TopLevel.GetTopLevel(this) is Window owner)
        {
            popup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            popup.Show(owner);
        }
        else
        {
            popup.Show();
        }
    }


    private void OnRemoveBookClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;

        var book = button.DataContext as Book;
        var month = button.FindAncestorOfType<Border>()?.DataContext as MonthBook;

        TimelineActions.RemoveFromMonth(month, book);

        e.Handled = true;
    }

    


    //TODO: Remove OnDragOver() & OnDrop

    private void OnDragOver(object? sender, DragEventArgs e)
    {
        Console.WriteLine("Timeline DragOver fired");

        if (e.Data.Contains("book"))
        {
            e.DragEffects = DragDropEffects.Copy;
            e.Handled = true;
        }
        else
        {
            e.DragEffects = DragDropEffects.None;
        }
    }

    private void OnDrop(object? sender, DragEventArgs e)
    {
        if (!e.Data.Contains("book") || sender is not Border border || border.DataContext is not MonthBook month)
            return;

        var book = e.Data.Get("book") as Book;
        if (book is null) return;

        System.Console.WriteLine($"Dropping '{book.Title}' onto {month.MonthText}");
        month.AssignBook(book);
        e.Handled = true;
       
    }
}
