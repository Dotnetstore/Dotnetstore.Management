using Dotnetstore.Management.SharedKernel.Domain;

namespace Dotnetstore.Management.Contacts.PersonCustomers;

internal sealed class PersonCustomer : BaseEntity
{
    public required string CustomerNumber { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Notes { get; set; }
}
