using Dotnetstore.Management.Contacts.ContactPersons;
using Dotnetstore.Management.Contacts.Data;
using Dotnetstore.Management.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Contacts.CompanyCustomers;

internal sealed class CompanyCustomerService(
    ContactsDataContext context,
    ICompanyCustomerRepository repository) : ICompanyCustomerService
{
    public async Task<Result<CompanyCustomerDto>> CreateAsync(
        CreateCompanyCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerNumber))
            return Result<CompanyCustomerDto>.Failure("Customer number is required.");
        if (string.IsNullOrWhiteSpace(request.CompanyName))
            return Result<CompanyCustomerDto>.Failure("Company name is required.");

        if (await repository.CustomerNumberExistsAsync(request.CustomerNumber, cancellationToken))
            return Result<CompanyCustomerDto>.Failure($"Customer number '{request.CustomerNumber}' already exists.");

        var entity = new CompanyCustomer
        {
            CustomerNumber = request.CustomerNumber,
            CompanyName = request.CompanyName,
            OrganizationNumber = request.OrganizationNumber,
            Email = request.Email,
            Phone = request.Phone,
            Street = request.Street,
            PostalCode = request.PostalCode,
            City = request.City,
            Country = request.Country,
            Notes = request.Notes,
        };

        context.CompanyCustomers.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Result<CompanyCustomerDto>.Success(Map(entity));
    }

    public async Task<CompanyCustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.CompanyCustomers
            .AsNoTracking()
            .Include(x => x.ContactPersons)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<IReadOnlyList<CompanyCustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await context.CompanyCustomers
            .AsNoTracking()
            .Include(x => x.ContactPersons)
            .OrderBy(x => x.CompanyName)
            .ToListAsync(cancellationToken);
        return list.Select(Map).ToList();
    }

    public async Task<Result<CompanyCustomerDto>> UpdateAsync(
        Guid id,
        UpdateCompanyCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName))
            return Result<CompanyCustomerDto>.Failure("Company name is required.");

        var entity = await context.CompanyCustomers
            .Include(x => x.ContactPersons)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
            return Result<CompanyCustomerDto>.Failure($"Company customer {id} not found.");

        entity.CompanyName = request.CompanyName;
        entity.OrganizationNumber = request.OrganizationNumber;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.Street = request.Street;
        entity.PostalCode = request.PostalCode;
        entity.City = request.City;
        entity.Country = request.Country;
        entity.Notes = request.Notes;
        entity.ModifiedAt = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
        return Result<CompanyCustomerDto>.Success(Map(entity));
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.CompanyCustomers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
            return Result<Guid>.Failure($"Company customer {id} not found.");

        context.CompanyCustomers.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(id);
    }

    public async Task<Result<ContactPersonDto>> AddContactPersonAsync(
        Guid companyId,
        AddContactPersonRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Firstname))
            return Result<ContactPersonDto>.Failure("Firstname is required.");
        if (string.IsNullOrWhiteSpace(request.Lastname))
            return Result<ContactPersonDto>.Failure("Lastname is required.");

        var exists = await context.CompanyCustomers
            .AsNoTracking()
            .AnyAsync(x => x.Id == companyId, cancellationToken);
        if (!exists)
            return Result<ContactPersonDto>.Failure($"Company customer {companyId} not found.");

        var contact = new ContactPerson
        {
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Title = request.Title,
            Email = request.Email,
            Phone = request.Phone,
            CompanyCustomerId = companyId,
        };

        context.ContactPersons.Add(contact);
        await context.SaveChangesAsync(cancellationToken);
        return Result<ContactPersonDto>.Success(MapContact(contact));
    }

    public async Task<Result<Guid>> RemoveContactPersonAsync(
        Guid companyId,
        Guid contactPersonId,
        CancellationToken cancellationToken = default)
    {
        var contact = await context.ContactPersons
            .FirstOrDefaultAsync(x => x.Id == contactPersonId && x.CompanyCustomerId == companyId, cancellationToken);
        if (contact is null)
            return Result<Guid>.Failure($"Contact person {contactPersonId} not found on company {companyId}.");

        context.ContactPersons.Remove(contact);
        await context.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(contactPersonId);
    }

    private static CompanyCustomerDto Map(CompanyCustomer e) => new(
        e.Id,
        e.CustomerNumber,
        e.CompanyName,
        e.OrganizationNumber,
        e.Email,
        e.Phone,
        e.Street,
        e.PostalCode,
        e.City,
        e.Country,
        e.Notes,
        e.ContactPersons.Select(MapContact).ToList());

    private static ContactPersonDto MapContact(ContactPerson c) => new(
        c.Id,
        c.CompanyCustomerId,
        c.Firstname,
        c.Lastname,
        c.Title,
        c.Email,
        c.Phone);
}
