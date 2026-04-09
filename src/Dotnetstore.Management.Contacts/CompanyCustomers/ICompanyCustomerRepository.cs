using Dotnetstore.Management.SharedKernel.Services;

namespace Dotnetstore.Management.Contacts.CompanyCustomers;

internal interface ICompanyCustomerRepository : IGenericRepository<CompanyCustomer>
{
    ValueTask<bool> CustomerNumberExistsAsync(string customerNumber, CancellationToken cancellationToken = default);
    ValueTask<CompanyCustomer?> GetWithContactsAsync(Guid id, CancellationToken cancellationToken = default);
}
