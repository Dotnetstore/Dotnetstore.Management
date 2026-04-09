using Dotnetstore.Management.Contacts.CompanyCustomers;
using Dotnetstore.Management.Contacts.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Dotnetstore.Management.Contacts.Tests.CompanyCustomers;

public class CompanyCustomerServiceTests
{
    private static CompanyCustomerService CreateService(SqliteTestDatabase db)
        => new(db.Context, new CompanyCustomerRepository(db.Context));

    [Fact]
    public async Task CreateAsync_persists_company()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);

        var result = await svc.CreateAsync(new CreateCompanyCustomerRequest(
            "C-001", "Acme Inc", OrganizationNumber: "123"));

        result.IsSuccess.ShouldBeTrue();
        result.Value!.CompanyName.ShouldBe("Acme Inc");
        result.Value.ContactPersons.ShouldBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_duplicate_customer_number_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        await svc.CreateAsync(new CreateCompanyCustomerRequest("C-001", "Acme"));

        (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-001", "Other")))
            .IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task CreateAsync_blank_company_name_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);

        (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-1", ""))).IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAsync_updates_fields()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var created = (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-1", "Acme"))).Value!;

        var result = await svc.UpdateAsync(created.Id, new UpdateCompanyCustomerRequest("Acme Ltd", OrganizationNumber: "999"));

        result.IsSuccess.ShouldBeTrue();
        result.Value!.CompanyName.ShouldBe("Acme Ltd");
        result.Value.OrganizationNumber.ShouldBe("999");
    }

    [Fact]
    public async Task UpdateAsync_nonexistent_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);

        (await svc.UpdateAsync(Guid.NewGuid(), new UpdateCompanyCustomerRequest("X"))).IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task AddContactPersonAsync_persists_contact_with_fk()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var company = (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-1", "Acme"))).Value!;

        var result = await svc.AddContactPersonAsync(company.Id,
            new AddContactPersonRequest("Ada", "Lovelace", Title: "CTO"));

        result.IsSuccess.ShouldBeTrue();
        result.Value!.CompanyCustomerId.ShouldBe(company.Id);
        result.Value.Title.ShouldBe("CTO");

        using var verify = db.CreateSecondaryContext();
        (await verify.ContactPersons.CountAsync()).ShouldBe(1);
    }

    [Fact]
    public async Task AddContactPersonAsync_unknown_company_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);

        (await svc.AddContactPersonAsync(Guid.NewGuid(), new AddContactPersonRequest("A", "B")))
            .IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task AddContactPersonAsync_blank_firstname_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var company = (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-1", "Acme"))).Value!;

        (await svc.AddContactPersonAsync(company.Id, new AddContactPersonRequest("", "B")))
            .IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task RemoveContactPersonAsync_removes_contact()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var company = (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-1", "Acme"))).Value!;
        var contact = (await svc.AddContactPersonAsync(company.Id, new AddContactPersonRequest("Ada", "Lovelace"))).Value!;

        (await svc.RemoveContactPersonAsync(company.Id, contact.Id)).IsSuccess.ShouldBeTrue();

        using var verify = db.CreateSecondaryContext();
        (await verify.ContactPersons.CountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task RemoveContactPersonAsync_unknown_contact_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var company = (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-1", "Acme"))).Value!;

        (await svc.RemoveContactPersonAsync(company.Id, Guid.NewGuid())).IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteAsync_cascades_to_contact_persons()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var company = (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-1", "Acme"))).Value!;
        await svc.AddContactPersonAsync(company.Id, new AddContactPersonRequest("Ada", "Lovelace"));
        await svc.AddContactPersonAsync(company.Id, new AddContactPersonRequest("Grace", "Hopper"));

        (await svc.DeleteAsync(company.Id)).IsSuccess.ShouldBeTrue();

        using var verify = db.CreateSecondaryContext();
        (await verify.CompanyCustomers.CountAsync()).ShouldBe(0);
        (await verify.ContactPersons.CountAsync()).ShouldBe(0);
    }

    [Fact]
    public async Task GetByIdAsync_includes_contact_persons()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var company = (await svc.CreateAsync(new CreateCompanyCustomerRequest("C-1", "Acme"))).Value!;
        await svc.AddContactPersonAsync(company.Id, new AddContactPersonRequest("Ada", "Lovelace"));

        var fetched = await svc.GetByIdAsync(company.Id);

        fetched.ShouldNotBeNull();
        fetched!.ContactPersons.Count.ShouldBe(1);
    }
}
