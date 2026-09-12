using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain;

namespace Users.Infrastructure.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(id => id.Value, value => UserId.From(value))
            .IsRequired();

        builder.Property(u => u.FirstName)
            .HasConversion(fn => fn.Value, value => FirstName.Create(value).Value)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasConversion(ln => ln.Value, value => LastName.Create(value).Value)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasConversion(e => e.Value, value => Email.Create(value).Value)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.IsActive)
            .IsRequired();

        // Auditing
        builder.Property(u => u.CreatedAtUtc)
            .IsRequired();

        builder.Property(u => u.CreatedBy)
            .HasMaxLength(100);

        builder.Property(u => u.LastModifiedAtUtc);

        builder.Property(u => u.LastModifiedBy)
            .HasMaxLength(100);

        // Soft delete
        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder.Property(u => u.DeletedAtUtc);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
