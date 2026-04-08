using CommunityToolkit.Mvvm.ComponentModel;
using Dotnetstore.Management.Organization.Data;
using Dotnetstore.Management.UI.GUI.Services;

namespace Dotnetstore.Management.UI.GUI.ViewModels;

public sealed partial class SplashViewModel(
    IDatabaseInitializer initializer,
    INavigationService navigation) : ViewModelBase
{
    [ObservableProperty]
    private double _progress;

    [ObservableProperty]
    private string _statusMessage = "Starting up...";

    [ObservableProperty]
    private string? _errorMessage;

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var progress = new Progress<InitializationProgress>(p =>
            {
                Progress = p.Value * 100.0;
                StatusMessage = p.Message;
            });

            await initializer.InitializeAsync(progress, cancellationToken);
            await Task.Delay(300, cancellationToken);
            navigation.ShowLogin();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Startup failed: {ex.Message}";
        }
    }
}
