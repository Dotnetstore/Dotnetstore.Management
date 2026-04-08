namespace Dotnetstore.Management.SharedKernel.Services;

public interface IGenericRepository<T> where T : class
{
    ValueTask<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    ValueTask<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);
    ValueTask InsertAsync(T entity, CancellationToken cancellationToken = default);
    ValueTask InsertAsync(List<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity);
    ValueTask DeleteAsync(object id, CancellationToken cancellationToken = default);
    ValueTask<int> SaveAsync(CancellationToken cancellationToken = default);
}