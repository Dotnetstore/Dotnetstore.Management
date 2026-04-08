using Dotnetstore.Management.Organization.Data;
using Dotnetstore.Management.Organization.Tests.Infrastructure;
using Dotnetstore.Management.SharedKernel.Security;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Dotnetstore.Management.Organization.Tests.Data;

public sealed class OrganizationDatabaseInitializerTests : IDisposable
{
    private readonly SqliteTestDatabase _db = new();
    private readonly OrganizationDatabaseInitializer _sut;

    public OrganizationDatabaseInitializerTests()
    {
        _sut = new OrganizationDatabaseInitializer(_db.Context, new Argon2PasswordHasher());
    }

    public void Dispose() => _db.Dispose();

    [Fact]
    public async Task InitializeAsync_SeedsInitialUser()
    {
        await _sut.InitializeAsync();

        using var verify = _db.CreateSecondaryContext();
        var user = await verify.Users.SingleAsync(u => u.Email == "hasse29@hotmail.com");
        user.Firstname.ShouldBe("Hans");
        user.Lastname.ShouldBe("Sjödin");
        user.IsActive.ShouldBeTrue();
        user.PasswordHash.Length.ShouldBe(64);
        user.PasswordSalt.Length.ShouldBe(16);
    }

    [Fact]
    public async Task InitializeAsync_RunTwice_DoesNotDuplicateInitialUser()
    {
        await _sut.InitializeAsync();
        await _sut.InitializeAsync();

        using var verify = _db.CreateSecondaryContext();
        (await verify.Users.CountAsync(u => u.Email == "hasse29@hotmail.com")).ShouldBe(1);
    }

    [Fact]
    public async Task InitializeAsync_ReportsProgressFromZeroToOne()
    {
        var values = new List<double>();
        var progress = new Progress<InitializationProgress>(p => values.Add(p.Value));

        await _sut.InitializeAsync(progress);

        await Task.Delay(50); // Progress posts asynchronously
        values.ShouldNotBeEmpty();
        values[^1].ShouldBe(1.0);
    }
}
