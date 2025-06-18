using Avalonia.Controls;
using PlotLineApp.ViewModels;

namespace PlotLineApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}