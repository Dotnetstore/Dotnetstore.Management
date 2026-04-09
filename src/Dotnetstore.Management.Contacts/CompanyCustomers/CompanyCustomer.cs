using Dotnetstore.Management.Contacts.ContactPersons;
using Dotnetstore.Management.SharedKernel.Domain;

namespace Dotnetstore.Management.Contacts.CompanyCustomers;

internal sealed class CompanyCustomer : BaseEntity
{
    public required string CustomerNumber { get; set; }
    public required string CompanyName { get; set; }
    public string? OrganizationNumber { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Notes { get; set; }

    public ICollection<ContactPerson> ContactPersons { get; set; } = new List<ContactPerson>();
}
