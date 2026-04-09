using Dotnetstore.Management.SharedKernel.Results;

namespace Dotnetstore.Management.Contacts.PersonCustomers;

public interface IPersonCustomerService
{
    Task<Result<PersonCustomerDto>> CreateAsync(CreatePersonCustomerRequest request, CancellationToken cancellationToken = default);
    Task<PersonCustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PersonCustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PersonCustomerDto>> UpdateAsync(Guid id, UpdatePersonCustomerRequest request, CancellationToken cancellationToken = default);
    Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public sealed record CreatePersonCustomerRequest(
    string CustomerNumber,
    string Firstname,
    string Lastname,
    string? Email = null,
    string? Phone = null,
    string? Street = null,
    string? PostalCode = null,
    string? City = null,
    string? Country = null,
    string? Notes = null);

public sealed record UpdatePersonCustomerRequest(
    string Firstname,
    string Lastname,
    string? Email = null,
    string? Phone = null,
    string? Street = null,
    string? PostalCode = null,
    string? City = null,
    string? Country = null,
    string? Notes = null);
