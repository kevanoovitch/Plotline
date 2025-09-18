using Avalonia.Controls;
using Avalonia.Input;
using PlotLineApp.Models;
using Plotline.DnD;
using System;

namespace PlotLineApp.Views
{
    public partial class BooksView : UserControl
    {


        public event Action<Book>? BookChosen;

        public void Book_DoubleTapped(object? sender, TappedEventArgs e)
        {
            if (sender is Border border && border.DataContext is Book book)
            {
                BookChosen?.Invoke(book);
            }

        }
    }
}

