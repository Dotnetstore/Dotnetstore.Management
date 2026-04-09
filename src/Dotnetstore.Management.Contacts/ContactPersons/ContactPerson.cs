using Dotnetstore.Management.SharedKernel.Domain;

namespace Dotnetstore.Management.Contacts.ContactPersons;

internal sealed class ContactPerson : BaseEntity
{
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public string? Title { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public Guid CompanyCustomerId { get; set; }
}
