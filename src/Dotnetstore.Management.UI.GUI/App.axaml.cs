using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Dotnetstore.Management.Organization.Extensions;
using Dotnetstore.Management.SharedKernel.Extensions;
using Dotnetstore.Management.UI.GUI.Services;
using Dotnetstore.Management.UI.GUI.ViewModels;
using Dotnetstore.Management.UI.GUI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnetstore.Management.UI.GUI;

public partial class App : Application
{
    private IServiceScope? _rootScope;
    public IServiceProvider? Services => _rootScope?.ServiceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var root = BuildServiceProvider();
            _rootScope = root.CreateScope();

            desktop.MainWindow = new SplashWindow
            {
                DataContext = _rootScope.ServiceProvider.GetRequiredService<SplashViewModel>(),
            };
            desktop.Exit += (_, _) => _rootScope.Dispose();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddSharedKernel();
        services.AddOrganizationModule(AppPaths.SqliteConnectionString);

        services.AddScoped<INavigationService, NavigationService>();
        services.AddScoped<SplashViewModel>();
        services.AddScoped<LoginViewModel>();
        services.AddScoped<MainWindowViewModel>();

        return services.BuildServiceProvider();
    }

}
