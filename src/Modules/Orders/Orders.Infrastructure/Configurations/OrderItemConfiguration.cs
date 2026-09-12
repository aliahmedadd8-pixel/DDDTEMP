using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain;

namespace Orders.Infrastructure.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasConversion(id => id.Value, value => OrderItemId.From(value))
            .IsRequired();

        builder.Property(i => i.OrderId)
            .HasConversion(id => id.Value, value => OrderId.From(value))
            .IsRequired();

        builder.Property(i => i.ProductName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();

        // Value Object: UnitPrice
        builder.ComplexProperty(i => i.UnitPrice, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount).HasColumnName("UnitPrice").HasPrecision(18, 2).IsRequired();
            moneyBuilder.Property(m => m.Currency).HasColumnName("UnitPriceCurrency").HasMaxLength(3).IsRequired();
        });

        // Value Object: TotalPrice
        builder.ComplexProperty(i => i.TotalPrice, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount).HasColumnName("TotalPrice").HasPrecision(18, 2).IsRequired();
            moneyBuilder.Property(m => m.Currency).HasColumnName("TotalPriceCurrency").HasMaxLength(3).IsRequired();
        });
    }
}
