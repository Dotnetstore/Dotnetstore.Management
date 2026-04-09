using Dotnetstore.Management.SharedKernel.Persistence;
using Dotnetstore.Management.UI.GUI.Services;
using Dotnetstore.Management.UI.GUI.ViewModels;
using NSubstitute;
using Shouldly;

namespace Dotnetstore.Management.UI.GUI.Tests.ViewModels;

public sealed class SplashViewModelTests
{
    private readonly IModuleDatabaseInitializer _first = Substitute.For<IModuleDatabaseInitializer>();
    private readonly IModuleDatabaseInitializer _second = Substitute.For<IModuleDatabaseInitializer>();
    private readonly INavigationService _navigation = Substitute.For<INavigationService>();

    [Fact]
    public async Task RunAsync_OnSuccess_RunsAllInitializersAndNavigatesToLogin()
    {
        _first.InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        _second.InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);
        var sut = new SplashViewModel(new[] { _first, _second }, _navigation);

        await sut.RunAsync();

        await _first.Received(1).InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>());
        await _second.Received(1).InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>());
        _navigation.Received(1).ShowLogin();
        sut.ErrorMessage.ShouldBeNull();
    }

    [Fact]
    public async Task RunAsync_WhenFirstInitializerFails_SecondNotRunAndErrorSet()
    {
        _first.InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>())
            .Returns<Task>(_ => throw new InvalidOperationException("boom"));
        var sut = new SplashViewModel(new[] { _first, _second }, _navigation);

        await sut.RunAsync();

        sut.ErrorMessage.ShouldNotBeNull();
        sut.ErrorMessage!.ShouldContain("boom");
        await _second.DidNotReceive().InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>());
        _navigation.DidNotReceive().ShowLogin();
    }

    [Fact]
    public async Task RunAsync_ReportsProgressAcrossSlices()
    {
        _first.InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>())
            .Returns(ci =>
            {
                ci.Arg<IProgress<InitializationProgress>>().Report(new InitializationProgress(1.0, "one"));
                return Task.CompletedTask;
            });
        _second.InitializeAsync(Arg.Any<IProgress<InitializationProgress>>(), Arg.Any<CancellationToken>())
            .Returns(ci =>
            {
                ci.Arg<IProgress<InitializationProgress>>().Report(new InitializationProgress(1.0, "two"));
                return Task.CompletedTask;
            });
        var sut = new SplashViewModel(new[] { _first, _second }, _navigation);

        await sut.RunAsync();
        await Task.Delay(50);

        sut.ProgressValue.ShouldBe(100.0);
    }
}
