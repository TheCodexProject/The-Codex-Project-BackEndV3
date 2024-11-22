

using OperationResult;

namespace unitTests.tools.OperationResultPattern;

[TestFixture]
public class ResultTests
{
    // # 1: A success result should have the `IsFailure` variable set to false, and it should have the `IsSuccess` as true.
    [Test]
    public void Success_Result_Should_Have_IsFailure_As_False()
    {
        // Arrange & Act
        var result = Result.Success();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.False);
            Assert.That(result.IsSuccess, Is.True);
        });
    }

    // # 2: A failure result should like wise have the `IsSuccess` variable set to false, and it should have the `IsFailure` as true.
    [Test]
    public void Failure_Result_Should_Have_IsSuccess_As_False()
    {
        // Arrange & Act
        var result = Result.Failure();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.IsFailure, Is.True);
        });
    }
}