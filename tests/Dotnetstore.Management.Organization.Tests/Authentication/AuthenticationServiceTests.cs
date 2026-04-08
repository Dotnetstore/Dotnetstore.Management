using Dotnetstore.Management.Organization.Authentication;
using Dotnetstore.Management.Organization.Tests.Infrastructure;
using Dotnetstore.Management.Organization.Users;
using Dotnetstore.Management.SharedKernel.Security;
using Shouldly;

namespace Dotnetstore.Management.Organization.Tests.Authentication;

public sealed class AuthenticationServiceTests : IDisposable
{
    private readonly SqliteTestDatabase _db = new();
    private readonly Argon2PasswordHasher _hasher = new();
    private readonly AuthenticationService _sut;

    public AuthenticationServiceTests()
    {
        _sut = new AuthenticationService(new UserRepository(_db.Context), _hasher);
    }

    public void Dispose() => _db.Dispose();

    private async Task<User> SeedAsync(string email, string password, bool isActive = true)
    {
        var hash = _hasher.Hash(password);
        var user = new User
        {
            Firstname = "Test",
            Lastname = "User",
            Email = email,
            PasswordHash = hash.Hash,
            PasswordSalt = hash.Salt,
            IsActive = isActive,
        };
        _db.Context.Users.Add(user);
        await _db.Context.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidCredentials_ReturnsSuccess()
    {
        await SeedAsync("hans@example.com", "admin");

        var result = await _sut.AuthenticateAsync("hans@example.com", "admin");

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value!.Email.ShouldBe("hans@example.com");
    }

    [Fact]
    public async Task AuthenticateAsync_WithWrongPassword_Fails()
    {
        await SeedAsync("hans@example.com", "admin");

        var result = await _sut.AuthenticateAsync("hans@example.com", "nope");

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task AuthenticateAsync_WithUnknownEmail_Fails()
    {
        var result = await _sut.AuthenticateAsync("ghost@example.com", "admin");

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task AuthenticateAsync_WithInactiveUser_Fails()
    {
        await SeedAsync("off@example.com", "admin", isActive: false);

        var result = await _sut.AuthenticateAsync("off@example.com", "admin");

        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error!.ShouldContain("not active");
    }

    [Theory]
    [InlineData("", "admin")]
    [InlineData("  ", "admin")]
    [InlineData("hans@example.com", "")]
    public async Task AuthenticateAsync_WithBlankInput_Fails(string email, string password)
    {
        var result = await _sut.AuthenticateAsync(email, password);

        result.IsSuccess.ShouldBeFalse();
    }
}
