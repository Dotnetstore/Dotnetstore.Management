using Dotnetstore.Management.Contacts.CompanyCustomers;
using Dotnetstore.Management.Contacts.ContactPersons;
using Dotnetstore.Management.Contacts.PersonCustomers;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Contacts.Data;

internal sealed class ContactsDataContext(DbContextOptions<ContactsDataContext> options) : DbContext(options)
{
    public DbSet<PersonCustomer> PersonCustomers => Set<PersonCustomer>();
    public DbSet<CompanyCustomer> CompanyCustomers => Set<CompanyCustomer>();
    public DbSet<ContactPerson> ContactPersons => Set<ContactPerson>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactsDataContext).Assembly);
    }
}
