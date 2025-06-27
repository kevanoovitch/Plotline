using System;

namespace PlotLineApp.Models {

public class MonthBook
{
    public DateTime Month {get; set;}
    public string BookTitle {get; set;} = string.Empty;
    public string MonthText => Month.ToString("MMMM");}

}   