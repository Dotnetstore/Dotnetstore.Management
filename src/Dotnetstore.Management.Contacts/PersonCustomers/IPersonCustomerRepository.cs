using Dotnetstore.Management.SharedKernel.Services;

namespace Dotnetstore.Management.Contacts.PersonCustomers;

internal interface IPersonCustomerRepository : IGenericRepository<PersonCustomer>
{
    ValueTask<bool> CustomerNumberExistsAsync(string customerNumber, CancellationToken cancellationToken = default);
}
