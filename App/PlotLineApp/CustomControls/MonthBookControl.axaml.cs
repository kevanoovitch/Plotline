using Avalonia.Controls;
using PlotLineApp.Models;
using System;

namespace PlotLineApp.CustomControls;

public partial class MonthBookControl : UserControl
{

    public MonthBook MonthData { get; set; }

    public MonthBookControl()
    {
        InitializeComponent();

        MonthData = new MonthBook
        {
            Month = DateTime.Now,
            BookTitle = "June book(?)"
        };

        DataContext = MonthData;
    }
}