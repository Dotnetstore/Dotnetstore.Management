using Dotnetstore.Management.Contacts.ContactPersons;

namespace Dotnetstore.Management.Contacts.CompanyCustomers;

public sealed record CompanyCustomerDto(
    Guid Id,
    string CustomerNumber,
    string CompanyName,
    string? OrganizationNumber,
    string? Email,
    string? Phone,
    string? Street,
    string? PostalCode,
    string? City,
    string? Country,
    string? Notes,
    IReadOnlyList<ContactPersonDto> ContactPersons);
