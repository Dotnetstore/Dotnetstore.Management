using Avalonia.Controls;
using Avalonia.Interactivity;
using Dotnetstore.Management.UI.GUI.ViewModels;

namespace Dotnetstore.Management.UI.GUI.Views;

public partial class SplashWindow : Window
{
    public SplashWindow()
    {
        InitializeComponent();
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        if (DataContext is SplashViewModel vm)
        {
            await vm.RunAsync();
        }
    }
}
