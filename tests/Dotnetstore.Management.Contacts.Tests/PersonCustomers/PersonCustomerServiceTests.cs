using Dotnetstore.Management.Contacts.PersonCustomers;
using Dotnetstore.Management.Contacts.Tests.Infrastructure;
using Shouldly;

namespace Dotnetstore.Management.Contacts.Tests.PersonCustomers;

public class PersonCustomerServiceTests
{
    private static PersonCustomerService CreateService(SqliteTestDatabase db)
        => new(db.Context, new PersonCustomerRepository(db.Context));

    [Fact]
    public async Task CreateAsync_persists_entity_and_returns_dto()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);

        var result = await svc.CreateAsync(new CreatePersonCustomerRequest(
            "P-001", "Ada", "Lovelace", Email: "ada@example.com"));

        result.IsSuccess.ShouldBeTrue();
        result.Value!.CustomerNumber.ShouldBe("P-001");
        result.Value.Firstname.ShouldBe("Ada");

        var fetched = await svc.GetByIdAsync(result.Value.Id);
        fetched.ShouldNotBeNull();
        fetched!.Email.ShouldBe("ada@example.com");
    }

    [Fact]
    public async Task CreateAsync_duplicate_customer_number_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        await svc.CreateAsync(new CreatePersonCustomerRequest("P-001", "Ada", "Lovelace"));

        var result = await svc.CreateAsync(new CreatePersonCustomerRequest("P-001", "Grace", "Hopper"));

        result.IsSuccess.ShouldBeFalse();
        result.Error!.ShouldContain("already exists");
    }

    [Fact]
    public async Task CreateAsync_blank_firstname_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);

        var result = await svc.CreateAsync(new CreatePersonCustomerRequest("P-1", "", "Hopper"));

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task UpdateAsync_updates_fields()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var created = (await svc.CreateAsync(new CreatePersonCustomerRequest("P-1", "Ada", "Lovelace"))).Value!;

        var result = await svc.UpdateAsync(created.Id, new UpdatePersonCustomerRequest("Ada", "Byron", Email: "ada@new"));

        result.IsSuccess.ShouldBeTrue();
        result.Value!.Lastname.ShouldBe("Byron");
        result.Value.Email.ShouldBe("ada@new");
    }

    [Fact]
    public async Task UpdateAsync_nonexistent_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);

        var result = await svc.UpdateAsync(Guid.NewGuid(), new UpdatePersonCustomerRequest("A", "B"));

        result.IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteAsync_removes_entity()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        var created = (await svc.CreateAsync(new CreatePersonCustomerRequest("P-1", "Ada", "Lovelace"))).Value!;

        (await svc.DeleteAsync(created.Id)).IsSuccess.ShouldBeTrue();
        (await svc.GetByIdAsync(created.Id)).ShouldBeNull();
    }

    [Fact]
    public async Task DeleteAsync_nonexistent_fails()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);

        (await svc.DeleteAsync(Guid.NewGuid())).IsSuccess.ShouldBeFalse();
    }

    [Fact]
    public async Task GetAllAsync_returns_all_ordered()
    {
        using var db = new SqliteTestDatabase();
        var svc = CreateService(db);
        await svc.CreateAsync(new CreatePersonCustomerRequest("P-1", "Ada", "Zeta"));
        await svc.CreateAsync(new CreatePersonCustomerRequest("P-2", "Grace", "Alpha"));

        var list = await svc.GetAllAsync();

        list.Count.ShouldBe(2);
        list[0].Lastname.ShouldBe("Alpha");
    }
}
