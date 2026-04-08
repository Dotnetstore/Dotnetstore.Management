using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.SharedKernel.Services;

public class GenericRepository<T>(DbContext context) : IGenericRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    async ValueTask<IEnumerable<T>> IGenericRepository<T>.GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet
            .ToListAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    async ValueTask<T?> IGenericRepository<T>.GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        return await _dbSet
            .FindAsync(id, cancellationToken)
            .ConfigureAwait(false);
    }

    async ValueTask IGenericRepository<T>.InsertAsync(T entity, CancellationToken cancellationToken)
    {
        await _dbSet
            .AddAsync(entity, cancellationToken)
            .ConfigureAwait(false);
    }

    async ValueTask IGenericRepository<T>.InsertAsync(List<T> entities, CancellationToken cancellationToken)
    {
        await _dbSet
            .AddRangeAsync(entities, cancellationToken)
            .ConfigureAwait(false);
    }

    void IGenericRepository<T>.Update(T entity)
    {
        _dbSet.Update(entity);
    }

    async ValueTask IGenericRepository<T>.DeleteAsync(object id, CancellationToken cancellationToken)
    {
        var entity = await _dbSet
            .FindAsync(id, cancellationToken)
            .ConfigureAwait(false);

        if (entity is not null)
        {
            _dbSet.Remove(entity);
        }
    }

    async ValueTask<int> IGenericRepository<T>.SaveAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken);
    }
}