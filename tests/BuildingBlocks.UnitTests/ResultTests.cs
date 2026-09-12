using BuildingBlocks.Domain;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.UnitTests;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        var result = Result.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().Be(Error.None);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult_WithError()
    {
        var error = Error.Validation("Code", "Description");
        var result = Result.Failure(error);

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void SuccessWithValue_ShouldContainValue()
    {
        var result = Result.Success(42);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void FailureWithValue_ShouldThrowWhenAccessingValue()
    {
        var error = Error.NotFound("Item.NotFound", "Item not found");
        var result = Result.Failure<int>(error);

        result.IsFailure.Should().BeTrue();
        var act = () => _ = result.Value;
        act.Should().Throw<InvalidOperationException>();
    }
}
