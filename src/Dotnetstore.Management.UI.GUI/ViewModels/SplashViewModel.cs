using CommunityToolkit.Mvvm.ComponentModel;
using Dotnetstore.Management.SharedKernel.Persistence;
using Dotnetstore.Management.UI.GUI.Services;

namespace Dotnetstore.Management.UI.GUI.ViewModels;

public sealed partial class SplashViewModel(
    IEnumerable<IModuleDatabaseInitializer> initializers,
    INavigationService navigation) : ViewModelBase
{
    [ObservableProperty]
    private double _progressValue;

    [ObservableProperty]
    private string _statusMessage = "Starting up...";

    [ObservableProperty]
    private string? _errorMessage;

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var list = initializers.ToList();
            if (list.Count == 0)
            {
                ProgressValue = 100.0;
                navigation.ShowLogin();
                return;
            }

            var slice = 1.0 / list.Count;
            for (var i = 0; i < list.Count; i++)
            {
                var baseValue = i * slice;
                var progress = new Progress<InitializationProgress>(p =>
                {
                    ProgressValue = (baseValue + p.Value * slice) * 100.0;
                    StatusMessage = p.Message;
                });

                await list[i].InitializeAsync(progress, cancellationToken);
            }

            await Task.Delay(300, cancellationToken);
            navigation.ShowLogin();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Startup failed: {ex.Message}";
        }
    }
}
