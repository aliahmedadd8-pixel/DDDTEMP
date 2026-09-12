using FluentAssertions;
using Users.Domain;
using Xunit;

namespace Users.UnitTests;

public class UserTests
{
    [Fact]
    public void CreateUser_WithValidData_ShouldSucceed_AndRaiseDomainEvent()
    {
        var firstName = FirstName.Create("John").Value;
        var lastName = LastName.Create("Doe").Value;
        var email = Email.Create("john.doe@example.com").Value;

        var result = User.Create(firstName, lastName, email);

        result.IsSuccess.Should().BeTrue();
        result.Value.FirstName.Should().Be(firstName);
        result.Value.LastName.Should().Be(lastName);
        result.Value.Email.Should().Be(email);
        result.Value.IsActive.Should().BeTrue();

        result.Value.DomainEvents.Should().ContainSingle();
        var domainEvent = result.Value.DomainEvents.First().Should().BeOfType<UserRegisteredDomainEvent>().Subject;
        domainEvent.UserId.Should().Be(result.Value.Id);
        domainEvent.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email")]
    [InlineData("missingat.com")]
    public void Email_WithInvalidFormat_ShouldFail(string invalidEmail)
    {
        var result = Email.Create(invalidEmail);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().StartWith("Email.");
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse_AndMarkAsDeleted()
    {
        var firstName = FirstName.Create("Jane").Value;
        var lastName = LastName.Create("Smith").Value;
        var email = Email.Create("jane.smith@example.com").Value;
        var user = User.Create(firstName, lastName, email).Value;

        user.Deactivate();

        user.IsActive.Should().BeFalse();
        user.IsDeleted.Should().BeTrue();
        user.DeletedAtUtc.Should().NotBeNull();
    }
}
