using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Dotnetstore.Management.Organization.Authentication;
using Dotnetstore.Management.UI.GUI.ViewModels;
using Dotnetstore.Management.UI.GUI.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnetstore.Management.UI.GUI.Services;

internal sealed class NavigationService(IServiceProvider services) : INavigationService
{
    public void ShowLogin()
    {
        var window = new LoginWindow
        {
            DataContext = services.GetRequiredService<LoginViewModel>(),
        };
        SwitchTo(window);
    }

    public void ShowMain(AuthenticatedUser user)
    {
        var vm = services.GetRequiredService<MainWindowViewModel>();
        vm.SetCurrentUser(user);
        var window = new MainWindow { DataContext = vm };
        SwitchTo(window);
    }

    private static void SwitchTo(Window next)
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;

        var previous = desktop.MainWindow;
        desktop.MainWindow = next;
        next.Show();
        previous?.Close();
    }
}
