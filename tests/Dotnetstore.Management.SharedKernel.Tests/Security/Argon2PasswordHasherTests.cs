using Dotnetstore.Management.SharedKernel.Security;
using Shouldly;

namespace Dotnetstore.Management.SharedKernel.Tests.Security;

public sealed class Argon2PasswordHasherTests
{
    private readonly Argon2PasswordHasher _sut = new();

    [Fact]
    public void Hash_ProducesNonEmptyHashAndSalt()
    {
        var result = _sut.Hash("admin");

        result.Hash.ShouldNotBeNull();
        result.Hash.Length.ShouldBe(64);
        result.Salt.ShouldNotBeNull();
        result.Salt.Length.ShouldBe(16);
    }

    [Fact]
    public void Hash_SamePasswordTwice_ProducesDifferentSaltsAndHashes()
    {
        var a = _sut.Hash("admin");
        var b = _sut.Hash("admin");

        a.Salt.ShouldNotBe(b.Salt);
        a.Hash.ShouldNotBe(b.Hash);
    }

    [Fact]
    public void Verify_WithCorrectPassword_ReturnsTrue()
    {
        var hash = _sut.Hash("s3cret!");

        _sut.Verify("s3cret!", hash.Hash, hash.Salt).ShouldBeTrue();
    }

    [Fact]
    public void Verify_WithWrongPassword_ReturnsFalse()
    {
        var hash = _sut.Hash("s3cret!");

        _sut.Verify("wrong", hash.Hash, hash.Salt).ShouldBeFalse();
    }

    [Fact]
    public void Verify_WithEmptyPassword_ReturnsFalse()
    {
        var hash = _sut.Hash("s3cret!");

        _sut.Verify(string.Empty, hash.Hash, hash.Salt).ShouldBeFalse();
    }

    [Fact]
    public void Hash_NullOrEmptyPassword_Throws()
    {
        Should.Throw<ArgumentException>(() => _sut.Hash(string.Empty));
    }
}
