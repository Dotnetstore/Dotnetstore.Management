namespace Dotnetstore.Management.Contacts.ContactPersons;

public sealed record ContactPersonDto(
    Guid Id,
    Guid CompanyCustomerId,
    string Firstname,
    string Lastname,
    string? Title,
    string? Email,
    string? Phone);
