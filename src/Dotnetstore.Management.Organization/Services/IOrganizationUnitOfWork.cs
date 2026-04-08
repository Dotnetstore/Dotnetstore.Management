using Dotnetstore.Management.Organization.Users;

namespace Dotnetstore.Management.Organization.Services;

internal interface IOrganizationUnitOfWork
{
    IUserRepository Users { get; }
    ValueTask CreateTransactionAsync();
    ValueTask CommitAsync();
    ValueTask RollbackAsync();
    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}