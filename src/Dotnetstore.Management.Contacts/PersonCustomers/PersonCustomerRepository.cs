using Dotnetstore.Management.Contacts.Data;
using Dotnetstore.Management.SharedKernel.Services;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Contacts.PersonCustomers;

internal sealed class PersonCustomerRepository(ContactsDataContext context)
    : GenericRepository<PersonCustomer>(context), IPersonCustomerRepository
{
    public async ValueTask<bool> CustomerNumberExistsAsync(string customerNumber, CancellationToken cancellationToken = default)
    {
        return await context.PersonCustomers
            .AnyAsync(x => x.CustomerNumber == customerNumber, cancellationToken)
            .ConfigureAwait(false);
    }
}
