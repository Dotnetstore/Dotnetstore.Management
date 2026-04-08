using Dotnetstore.Management.Organization.Data;
using Dotnetstore.Management.Organization.Users;
using Microsoft.EntityFrameworkCore.Storage;

namespace Dotnetstore.Management.Organization.Services;

internal class OrganizationUnitOfWork(
    OrganizationDataContext context,
    IUserRepository userRepository) : IOrganizationUnitOfWork, IDisposable
{
    private IDbContextTransaction? _objTran;
    private bool _disposed;

    IUserRepository IOrganizationUnitOfWork.Users => userRepository;

    async ValueTask IOrganizationUnitOfWork.CreateTransactionAsync()
    {
        _objTran = await context.Database.BeginTransactionAsync();
    }

    async ValueTask IOrganizationUnitOfWork.CommitAsync()
    {
        if (_objTran is null) return;
        await _objTran!.CommitAsync();
    }

    async ValueTask IOrganizationUnitOfWork.RollbackAsync()
    {
        if (_objTran is null) return;
        await _objTran!.RollbackAsync();
        await _objTran.DisposeAsync();
    }

    async ValueTask<int> IOrganizationUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(OrganizationUnitOfWork));
        return await context.SaveChangesAsync(cancellationToken);
    }

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            context.Dispose();
            _objTran?.Dispose();
        }

        _disposed = true;
    }
}