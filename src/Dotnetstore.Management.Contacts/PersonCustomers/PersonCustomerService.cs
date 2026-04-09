using Dotnetstore.Management.Contacts.Data;
using Dotnetstore.Management.SharedKernel.Results;
using Microsoft.EntityFrameworkCore;

namespace Dotnetstore.Management.Contacts.PersonCustomers;

internal sealed class PersonCustomerService(
    ContactsDataContext context,
    IPersonCustomerRepository repository) : IPersonCustomerService
{
    public async Task<Result<PersonCustomerDto>> CreateAsync(
        CreatePersonCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerNumber))
            return Result<PersonCustomerDto>.Failure("Customer number is required.");
        if (string.IsNullOrWhiteSpace(request.Firstname))
            return Result<PersonCustomerDto>.Failure("Firstname is required.");
        if (string.IsNullOrWhiteSpace(request.Lastname))
            return Result<PersonCustomerDto>.Failure("Lastname is required.");

        if (await repository.CustomerNumberExistsAsync(request.CustomerNumber, cancellationToken))
            return Result<PersonCustomerDto>.Failure($"Customer number '{request.CustomerNumber}' already exists.");

        var entity = new PersonCustomer
        {
            CustomerNumber = request.CustomerNumber,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Email = request.Email,
            Phone = request.Phone,
            Street = request.Street,
            PostalCode = request.PostalCode,
            City = request.City,
            Country = request.Country,
            Notes = request.Notes,
        };

        context.PersonCustomers.Add(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Result<PersonCustomerDto>.Success(Map(entity));
    }

    public async Task<PersonCustomerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.PersonCustomers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return entity is null ? null : Map(entity);
    }

    public async Task<IReadOnlyList<PersonCustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = await context.PersonCustomers
            .AsNoTracking()
            .OrderBy(x => x.Lastname).ThenBy(x => x.Firstname)
            .ToListAsync(cancellationToken);
        return list.Select(Map).ToList();
    }

    public async Task<Result<PersonCustomerDto>> UpdateAsync(
        Guid id,
        UpdatePersonCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Firstname))
            return Result<PersonCustomerDto>.Failure("Firstname is required.");
        if (string.IsNullOrWhiteSpace(request.Lastname))
            return Result<PersonCustomerDto>.Failure("Lastname is required.");

        var entity = await context.PersonCustomers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
            return Result<PersonCustomerDto>.Failure($"Person customer {id} not found.");

        entity.Firstname = request.Firstname;
        entity.Lastname = request.Lastname;
        entity.Email = request.Email;
        entity.Phone = request.Phone;
        entity.Street = request.Street;
        entity.PostalCode = request.PostalCode;
        entity.City = request.City;
        entity.Country = request.Country;
        entity.Notes = request.Notes;
        entity.ModifiedAt = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
        return Result<PersonCustomerDto>.Success(Map(entity));
    }

    public async Task<Result<Guid>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await context.PersonCustomers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (entity is null)
            return Result<Guid>.Failure($"Person customer {id} not found.");

        context.PersonCustomers.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(id);
    }

    private static PersonCustomerDto Map(PersonCustomer e) => new(
        e.Id,
        e.CustomerNumber,
        e.Firstname,
        e.Lastname,
        e.Email,
        e.Phone,
        e.Street,
        e.PostalCode,
        e.City,
        e.Country,
        e.Notes);
}
