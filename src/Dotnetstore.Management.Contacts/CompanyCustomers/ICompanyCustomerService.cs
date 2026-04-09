using Dotnetstore.Management.Contacts.ContactPersons;
using Dotnetstore.Management.SharedKernel.Results;

namespace Dotnetstore.Management.Contacts.CompanyCustomers;

public interface ICompanyCustomerService
{
    Task<Result<CompanyCustomerDto>> CreateAsync(CreateCompanyCustomerRequest request, CancellationToken cancellationToken = default);
    Task<CompanyCustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CompanyCustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<CompanyCustomerDto>> UpdateAsync(Guid id, UpdateCompanyCustomerRequest request, CancellationToken cancellationToken = default);
    Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<ContactPersonDto>> AddContactPersonAsync(Guid companyId, AddContactPersonRequest request, CancellationToken cancellationToken = default);
    Task<Result<Guid>> RemoveContactPersonAsync(Guid companyId, Guid contactPersonId, CancellationToken cancellationToken = default);
}

public sealed record CreateCompanyCustomerRequest(
    string CustomerNumber,
    string CompanyName,
    string? OrganizationNumber = null,
    string? Email = null,
    string? Phone = null,
    string? Street = null,
    string? PostalCode = null,
    string? City = null,
    string? Country = null,
    string? Notes = null);

public sealed record UpdateCompanyCustomerRequest(
    string CompanyName,
    string? OrganizationNumber = null,
    string? Email = null,
    string? Phone = null,
    string? Street = null,
    string? PostalCode = null,
    string? City = null,
    string? Country = null,
    string? Notes = null);

public sealed record AddContactPersonRequest(
    string Firstname,
    string Lastname,
    string? Title = null,
    string? Email = null,
    string? Phone = null);
