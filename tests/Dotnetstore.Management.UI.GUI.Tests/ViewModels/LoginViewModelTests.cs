using Dotnetstore.Management.Organization.Authentication;
using Dotnetstore.Management.SharedKernel.Results;
using Dotnetstore.Management.UI.GUI.Services;
using Dotnetstore.Management.UI.GUI.ViewModels;
using NSubstitute;
using Shouldly;

namespace Dotnetstore.Management.UI.GUI.Tests.ViewModels;

public sealed class LoginViewModelTests
{
    private readonly IAuthenticationService _auth = Substitute.For<IAuthenticationService>();
    private readonly INavigationService _navigation = Substitute.For<INavigationService>();
    private readonly LoginViewModel _sut;

    public LoginViewModelTests()
    {
        _sut = new LoginViewModel(_auth, _navigation);
    }

    [Fact]
    public void LoginCommand_CannotExecute_WhenEmailOrPasswordEmpty()
    {
        _sut.LoginCommand.CanExecute(null).ShouldBeFalse();

        _sut.Email = "hans@example.com";
        _sut.LoginCommand.CanExecute(null).ShouldBeFalse();

        _sut.Password = "admin";
        _sut.LoginCommand.CanExecute(null).ShouldBeTrue();
    }

    [Fact]
    public async Task LoginAsync_OnSuccess_NavigatesToMain()
    {
        var user = new AuthenticatedUser(Guid.NewGuid(), "Hans", "Sjödin", "hans@example.com");
        _auth.AuthenticateAsync("hans@example.com", "admin", Arg.Any<CancellationToken>())
            .Returns(Result<AuthenticatedUser>.Success(user));
        _sut.Email = "hans@example.com";
        _sut.Password = "admin";

        await _sut.LoginCommand.ExecuteAsync(null);

        _navigation.Received(1).ShowMain(user);
        _sut.ErrorMessage.ShouldBeNull();
    }

    [Fact]
    public async Task LoginAsync_OnFailure_SetsErrorMessageAndDoesNotNavigate()
    {
        _auth.AuthenticateAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result<AuthenticatedUser>.Failure("Invalid email or password."));
        _sut.Email = "hans@example.com";
        _sut.Password = "wrong";

        await _sut.LoginCommand.ExecuteAsync(null);

        _sut.ErrorMessage.ShouldBe("Invalid email or password.");
        _navigation.DidNotReceive().ShowMain(Arg.Any<AuthenticatedUser>());
    }
}
