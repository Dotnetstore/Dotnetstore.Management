using Dotnetstore.Management.Organization.Authentication;

namespace Dotnetstore.Management.UI.GUI.Services;

public interface INavigationService
{
    void ShowLogin();
    void ShowMain(AuthenticatedUser user);
}
