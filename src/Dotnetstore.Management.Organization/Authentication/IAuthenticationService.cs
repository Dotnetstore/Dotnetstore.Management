using Dotnetstore.Management.SharedKernel.Results;

namespace Dotnetstore.Management.Organization.Authentication;

public interface IAuthenticationService
{
    Task<Result<AuthenticatedUser>> AuthenticateAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);
}

public sealed record AuthenticatedUser(Guid Id, string Firstname, string Lastname, string Email);
