using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dotnetstore.Management.Organization.Authentication;
using Dotnetstore.Management.UI.GUI.Services;

namespace Dotnetstore.Management.UI.GUI.ViewModels;

public sealed partial class LoginViewModel(
    IAuthenticationService authentication,
    INavigationService navigation) : ViewModelBase
{
    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private bool _isBusy;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync(CancellationToken cancellationToken)
    {
        ErrorMessage = null;
        IsBusy = true;
        try
        {
            var result = await authentication.AuthenticateAsync(Email, Password, cancellationToken);
            if (!result.IsSuccess || result.Value is null)
            {
                ErrorMessage = result.Error;
                return;
            }
            navigation.ShowMain(result.Value);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanLogin() => !IsBusy && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrEmpty(Password);

    partial void OnEmailChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
    partial void OnPasswordChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
    partial void OnIsBusyChanged(bool value) => LoginCommand.NotifyCanExecuteChanged();
}
