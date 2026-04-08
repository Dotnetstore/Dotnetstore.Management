using Dotnetstore.Management.Organization.Data;
using Dotnetstore.Management.UI.GUI.Services;
using Dotnetstore.Management.UI.GUI.ViewModels;
using NSubstitute;
using Shouldly;

namespace Dotnetstore.Management.UI.GUI.Tests.ViewModels;

public sealed class SplashViewModelTests
{
    private readonly IDatabaseInitializer _initializer = Substitute.For<IDatabaseInitializer>();
    private readonly INavigationService _navigation = Substitute.For<INavigationService>();

    [Fact]
    public async Task RunAsync_OnSuccess_NavigatesToLogin()
    {
        _initializer.InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        var sut = new SplashViewModel(_initializer, _navigation);

        await sut.RunAsync();

        _navigation.Received(1).ShowLogin();
        sut.ErrorMessage.ShouldBeNull();
    }

    [Fact]
    public async Task RunAsync_OnInitializerFailure_SetsErrorAndDoesNotNavigate()
    {
        _initializer.InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>())
            .Returns<Task>(_ => throw new InvalidOperationException("boom"));
        var sut = new SplashViewModel(_initializer, _navigation);

        await sut.RunAsync();

        sut.ErrorMessage.ShouldNotBeNull();
        sut.ErrorMessage!.ShouldContain("boom");
        _navigation.DidNotReceive().ShowLogin();
    }
}
