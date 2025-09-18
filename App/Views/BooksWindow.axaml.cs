using Avalonia.Controls;
using Avalonia.Input;
using PlotLineApp.Models;
using Plotline.DnD;
using System;
using PlotLineApp.ViewModels;
using Avalonia.Interactivity;
namespace PlotLineApp.Views

{
    public partial class BooksWindow : Window
    {

        public event Action<Book>? BookChosen;
        public BooksWindow()
        {
            InitializeComponent();
        }

        public void Book_DoubleTapped(object? sender, RoutedEventArgs e)
        {
            if (sender is Border border && border.DataContext is Book book)
            {
                BookChosen?.Invoke(book);
            }
        }

        public void OnClearSearchClick(object? sender, RoutedEventArgs e)
        {
            if (DataContext is BooksViewModel vm)
                vm.SearchTerm = string.Empty;
        }

    }
    
   
}
