using Dotnetstore.Management.Contacts.ContactPersons;
using Dotnetstore.Management.Contacts.Tests.Infrastructure;
using Shouldly;

namespace Dotnetstore.Management.Contacts.Tests.ContactPersons;

public class ContactPersonFkTests
{
    [Fact]
    public async Task Insert_with_unknown_company_fk_throws()
    {
        using var db = new SqliteTestDatabase();
        db.Context.ContactPersons.Add(new ContactPerson
        {
            Firstname = "Ada",
            Lastname = "Lovelace",
            CompanyCustomerId = Guid.NewGuid(),
        });

        await Should.ThrowAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(
            async () => await db.Context.SaveChangesAsync());
    }
}
