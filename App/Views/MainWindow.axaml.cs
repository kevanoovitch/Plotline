using Avalonia.Controls;
using PlotLineApp.ViewModels;
using PlotLineApp.Services;
using System;
using System.Linq;
using Avalonia.Controls.Primitives;


namespace PlotLineApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainWindowViewModel(new AppCloser());
        DataContext = viewModel;

        viewModel.OpenBooksPopupRequested += (_, _) =>
        {
            Console.WriteLine("Popup requested!");
            var popup = new BooksWindow
            {
                DataContext = new BooksViewModel(() => { })
            };

            popup.Show(this);
        };

        
       
    }
}
