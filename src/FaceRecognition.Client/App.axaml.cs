using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FaceRecognition.Client.Services;
using FaceRecognition.Client.ViewModels;
using FaceRecognition.Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace FaceRecognition.Client;

public partial class App : Application
{
    private ServiceProvider _serviceProvider;
    public override void Initialize()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        _serviceProvider = serviceCollection.BuildServiceProvider();
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Add new services here
    /// </summary>
    /// <param name="services"></param>
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<IOService, DialogService>();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            desktop.MainWindow.DataContext = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        }

        base.OnFrameworkInitializationCompleted();
    }
}