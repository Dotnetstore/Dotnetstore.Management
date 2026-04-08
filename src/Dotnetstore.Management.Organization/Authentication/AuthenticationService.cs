using Dotnetstore.Management.Organization.Users;
using Dotnetstore.Management.SharedKernel.Results;
using Dotnetstore.Management.SharedKernel.Security;

namespace Dotnetstore.Management.Organization.Authentication;

internal sealed class AuthenticationService(
    IUserRepository users,
    IPasswordHasher passwordHasher) : IAuthenticationService
{
    private const string InvalidCredentials = "Invalid email or password.";
    private const string InactiveAccount = "This account is not active.";

    public async Task<Result<AuthenticatedUser>> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrEmpty(password))
            return Result<AuthenticatedUser>.Failure(InvalidCredentials);

        var user = await users.GetByEmailAsync(email, cancellationToken);
        if (user is null)
            return Result<AuthenticatedUser>.Failure(InvalidCredentials);

        if (!user.IsActive)
            return Result<AuthenticatedUser>.Failure(InactiveAccount);

        if (!passwordHasher.Verify(password, user.PasswordHash, user.PasswordSalt))
            return Result<AuthenticatedUser>.Failure(InvalidCredentials);

        return Result<AuthenticatedUser>.Success(
            new AuthenticatedUser(user.Id, user.Firstname, user.Lastname, user.Email));
    }
}
