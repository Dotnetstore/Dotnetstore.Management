using CommunityToolkit.Mvvm.ComponentModel;
using Dotnetstore.Management.Organization.Authentication;

namespace Dotnetstore.Management.UI.GUI.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Dotnetstore.Management";

    public void SetCurrentUser(AuthenticatedUser user)
    {
        Greeting = $"Welcome, {user.Firstname} {user.Lastname}";
    }
}
