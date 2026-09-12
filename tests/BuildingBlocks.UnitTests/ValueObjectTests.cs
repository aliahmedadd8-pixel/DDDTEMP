using BuildingBlocks.Domain;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.UnitTests;

public sealed class TestAddress : ValueObject
{
    public TestAddress(string street, string city)
    {
        Street = street;
        City = city;
    }

    public string Street { get; }
    public string City { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
    }
}

public class ValueObjectTests
{
    [Fact]
    public void TwoValueObjects_WithSameValues_ShouldBeEqual()
    {
        var address1 = new TestAddress("Main St", "Cairo");
        var address2 = new TestAddress("Main St", "Cairo");

        (address1 == address2).Should().BeTrue();
        address1.Equals(address2).Should().BeTrue();
        address1.GetHashCode().Should().Be(address2.GetHashCode());
    }

    [Fact]
    public void TwoValueObjects_WithDifferentValues_ShouldNotBeEqual()
    {
        var address1 = new TestAddress("Main St", "Cairo");
        var address2 = new TestAddress("Second St", "Riyadh");

        (address1 == address2).Should().BeFalse();
        (address1 != address2).Should().BeTrue();
    }
}
