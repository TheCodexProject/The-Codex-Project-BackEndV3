using domain.exceptions;
using OperationResult;

namespace unitTests.tools.OperationResultPattern;

[TestFixture]
public class ResultsOfTTests
{
    [Test]
    public void Success_Result_Create_Result_With_Value()
    {
        // Arrange
        const int value = 10;

        // Act
        var result = Result<int>.Success(value);

        // Assert
        Assert.That(result.Value, Is.EqualTo(value));
    }

    [Test]
    public void Success_Result_Create_Result_With_Value_IsFailure_Should_Be_False()
    {
        // Arrange
        const int value = 10;

        // Act
        var result = Result<int>.Success(value);

        // Assert
        Assert.That(result.IsFailure, Is.False);
    }

    [Test]
    public void Failure_Result_Create_Result_With_Error_Message_IsFailure_Should_Be_True()
    {
        // Arrange
        var userNotFound = new NotFoundException("User could not be found.");

        // Act
        var result = Result<int>.Failure(userNotFound);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    [Test]
    public void Implicit_Conversion_From_T_To_Result_T()
    {
        // Arrange
        const int value = 10;

        // Act
        Result<int> result = value;

        // Assert
        Assert.That(result.Value, Is.EqualTo(value));
    }

    [Test]
    public void Implicit_Conversion_From_Result_T_To_T()
    {
        // Arrange
        const int value = 10;
        var result = Result<int>.Success(value);

        // Act
        int resultValue = result;

        // Assert
        Assert.That(resultValue, Is.EqualTo(value));
    }

}