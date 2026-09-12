using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain;

namespace Orders.Infrastructure.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(id => id.Value, value => OrderId.From(value))
            .IsRequired();

        builder.Property(o => o.CustomerId)
            .HasConversion(id => id.Value, value => CustomerId.From(value))
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        // Value Object: ShippingAddress
        builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
        {
            addressBuilder.Property(a => a.Street).HasColumnName("ShippingStreet").HasMaxLength(200).IsRequired();
            addressBuilder.Property(a => a.City).HasColumnName("ShippingCity").HasMaxLength(100).IsRequired();
            addressBuilder.Property(a => a.Country).HasColumnName("ShippingCountry").HasMaxLength(100).IsRequired();
            addressBuilder.Property(a => a.ZipCode).HasColumnName("ShippingZipCode").HasMaxLength(20).IsRequired();
        });

        // Value Object: TotalAmount
        builder.ComplexProperty(o => o.TotalAmount, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount).HasColumnName("TotalAmount").HasPrecision(18, 2).IsRequired();
            moneyBuilder.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3).IsRequired();
        });

        // Navigation to OrderItems
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Auditing
        builder.Property(o => o.CreatedAtUtc).IsRequired();
        builder.Property(o => o.CreatedBy).HasMaxLength(100);
        builder.Property(o => o.LastModifiedAtUtc);
        builder.Property(o => o.LastModifiedBy).HasMaxLength(100);

        // Soft delete
        builder.Property(o => o.IsDeleted).HasDefaultValue(false);
        builder.Property(o => o.DeletedAtUtc);

        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}
