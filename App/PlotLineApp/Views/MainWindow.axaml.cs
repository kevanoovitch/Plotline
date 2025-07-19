using Avalonia.Controls;
using PlotLineApp.ViewModels;
using PlotLineApp.Services;

namespace PlotLineApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(new AppCloser());
    }
}