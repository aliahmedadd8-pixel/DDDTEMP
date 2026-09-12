using FluentAssertions;
using Orders.Domain;
using Xunit;

namespace Orders.UnitTests;

public class OrderTests
{
    [Fact]
    public void Money_AddSameCurrency_ShouldSucceed()
    {
        var m1 = Money.Create(100m, "USD").Value;
        var m2 = Money.Create(50m, "USD").Value;

        var result = m1.Add(m2);

        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(150m);
        result.Value.Currency.Should().Be("USD");
    }

    [Fact]
    public void Money_AddDifferentCurrencies_ShouldFail()
    {
        var m1 = Money.Create(100m, "USD").Value;
        var m2 = Money.Create(50m, "EUR").Value;

        var result = m1.Add(m2);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Money.CurrencyMismatch");
    }

    [Fact]
    public void Order_AddItem_ShouldRecalculateTotalAmount()
    {
        var customerId = CustomerId.New();
        var address = Address.Create("Street", "City", "Country", "12345").Value;
        var order = Order.Create(customerId, address).Value;

        var price1 = Money.Create(20m, "USD").Value;
        var price2 = Money.Create(15m, "USD").Value;

        order.AddItem("Product A", price1, 2); // 40
        order.AddItem("Product B", price2, 1); // 15

        order.TotalAmount.Amount.Should().Be(55m);
        order.Items.Should().HaveCount(2);
    }

    [Fact]
    public void Order_CompleteCreation_WithItems_ShouldRaiseOrderCreatedDomainEvent()
    {
        var customerId = CustomerId.New();
        var address = Address.Create("Street", "City", "Country", "12345").Value;
        var order = Order.Create(customerId, address).Value;

        var price = Money.Create(50m, "USD").Value;
        order.AddItem("Product A", price, 1);

        var result = order.CompleteCreation();

        result.IsSuccess.Should().BeTrue();
        order.DomainEvents.Should().ContainSingle();
        var domainEvent = order.DomainEvents.First().Should().BeOfType<OrderCreatedDomainEvent>().Subject;
        domainEvent.OrderId.Should().Be(order.Id);
        domainEvent.CustomerId.Should().Be(customerId);
        domainEvent.TotalAmount.Amount.Should().Be(50m);
    }

    [Fact]
    public void Order_CompleteCreation_WithoutItems_ShouldFail()
    {
        var customerId = CustomerId.New();
        var address = Address.Create("Street", "City", "Country", "12345").Value;
        var order = Order.Create(customerId, address).Value;

        var result = order.CompleteCreation();

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(OrderErrors.EmptyItems);
    }

    [Fact]
    public void Order_Cancel_WhenPending_ShouldSucceed_AndRaiseDomainEvent()
    {
        var customerId = CustomerId.New();
        var address = Address.Create("Street", "City", "Country", "12345").Value;
        var order = Order.Create(customerId, address).Value;

        var cancelResult = order.Cancel();

        cancelResult.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Cancelled);
        order.DomainEvents.Should().ContainSingle();
        var domainEvent = order.DomainEvents.First().Should().BeOfType<OrderCancelledDomainEvent>().Subject;
        domainEvent.OrderId.Should().Be(order.Id);
    }

    [Fact]
    public void Order_Cancel_WhenAlreadyCancelled_ShouldFail()
    {
        var customerId = CustomerId.New();
        var address = Address.Create("Street", "City", "Country", "12345").Value;
        var order = Order.Create(customerId, address).Value;
        order.Cancel();

        var secondCancel = order.Cancel();

        secondCancel.IsFailure.Should().BeTrue();
        secondCancel.Error.Should().Be(OrderErrors.AlreadyCancelled);
    }
}
