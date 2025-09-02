using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using PlotLineApp.Services;

public class AppCloser : IAppCloser
{
    public void Shutdown()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.Shutdown();
        }
    }
}