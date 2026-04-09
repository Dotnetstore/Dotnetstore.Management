using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dotnetstore.Management.Contacts.ContactPersons;

internal sealed class ContactPersonConfiguration : IEntityTypeConfiguration<ContactPerson>
{
    public void Configure(EntityTypeBuilder<ContactPerson> builder)
    {
        builder.ToTable("ContactPersons");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Firstname).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Lastname).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Title).HasMaxLength(100);
        builder.Property(x => x.Email).HasMaxLength(256);
        builder.Property(x => x.Phone).HasMaxLength(64);
        builder.Property(x => x.CompanyCustomerId).IsRequired();
        builder.HasIndex(x => x.CompanyCustomerId);
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
