using Dotnetstore.Management.Contacts.Data;
using Dotnetstore.Management.SharedKernel.Services;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Contacts.CompanyCustomers;

internal sealed class CompanyCustomerRepository(ContactsDataContext context)
    : GenericRepository<CompanyCustomer>(context), ICompanyCustomerRepository
{
    public async ValueTask<bool> CustomerNumberExistsAsync(string customerNumber, CancellationToken cancellationToken = default)
    {
        return await context.CompanyCustomers
            .AnyAsync(x => x.CustomerNumber == customerNumber, cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<CompanyCustomer?> GetWithContactsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.CompanyCustomers
            .Include(x => x.ContactPersons)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }
}
