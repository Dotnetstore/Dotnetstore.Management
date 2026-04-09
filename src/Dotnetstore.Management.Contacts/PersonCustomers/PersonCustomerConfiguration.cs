using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnetstore.Management.Contacts.PersonCustomers;

internal sealed class PersonCustomerConfiguration : IEntityTypeConfiguration<PersonCustomer>
{
    public void Configure(EntityTypeBuilder<PersonCustomer> builder)
    {
        builder.ToTable("PersonCustomers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerNumber).IsRequired().HasMaxLength(32);
        builder.HasIndex(x => x.CustomerNumber).IsUnique();
        builder.Property(x => x.Firstname).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Lastname).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Phone).HasMaxLength(64);
        builder.Property(x => x.Street).HasMaxLength(200);
        builder.Property(x => x.PostalCode).HasMaxLength(32);
        builder.Property(x => x.City).HasMaxLength(100);
        builder.Property(x => x.Country).HasMaxLength(100);
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
