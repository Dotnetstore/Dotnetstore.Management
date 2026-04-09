namespace Dotnetstore.Management.Contacts.PersonCustomers;

public sealed record PersonCustomerDto(
    Guid Id,
    string CustomerNumber,
    string Firstname,
    string Lastname,
    string? Email,
    string? Phone,
    string? Street,
    string? PostalCode,
    string? City,
    string? Country,
    string? Notes);
