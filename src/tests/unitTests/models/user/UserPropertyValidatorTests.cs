using domain.models.user;

namespace unitTests.models.user;

[TestFixture]
public class UserPropertyValidatorTests
{
    // SECTION #1: First Name

    // # 1: Empty, whitespace or null should be invalid
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Empty_FirstName_Should_Be_Invalid(string? value)
    {
        // Act
        var result = UserPropertyValidator.ValidateFirstName(value!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: First name cannot contain any non-alphabetic characters
    [Theory]
    [TestCase("John1")]
    [TestCase("John.Doe")]
    [TestCase("John@Doe")]
    public void FirstName_With_NonAlphabetic_Characters_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateFirstName(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: First name cannot contain extra spaces
    [Theory]
    [TestCase("John  Doe")]
    [TestCase(" John")]
    [TestCase("John ")]
    public void FirstName_With_Extra_Spaces_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateFirstName(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: First name should be at least 2 characters long
    [Theory]
    [TestCase("J")]
    [TestCase("A")]
    public void FirstName_Too_Short_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateFirstName(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 5: First name should be at most 50 characters long
    [Test]
    public void FirstName_Too_Long_Should_Be_Invalid()
    {
        // Arrange
        var value = "John".PadLeft(50);

        // Act
        var result = UserPropertyValidator.ValidateFirstName(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // SECTION #2: Last Name

    // # 1: Empty, whitespace or null should be invalid
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Empty_LastName_Should_Be_Invalid(string? value)
    {
        // Act
        var result = UserPropertyValidator.ValidateLastName(value!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Last name cannot contain any non-alphabetic characters
    [Theory]
    [TestCase("Doe1")]
    [TestCase("Doe.Doe")]
    [TestCase("Doe@Doe")]
    public void LastName_With_NonAlphabetic_Characters_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateLastName(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Last name cannot contain extra spaces
    [Theory]
    [TestCase("Doe  Doe")]
    [TestCase(" Doe")]
    [TestCase("Doe ")]
    public void LastName_With_Extra_Spaces_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateLastName(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: Last name should be at least 2 characters long
    [Theory]
    [TestCase("D")]
    [TestCase("A")]
    public void LastName_Too_Short_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateLastName(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 5: Last name should be at most 60 characters long
    [Test]
    public void LastName_Too_Long_Should_Be_Invalid()
    {
        // Arrange
        var value = "Doe".PadLeft(60);

        // Act
        var result = UserPropertyValidator.ValidateLastName(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // SECTION #3: Email

    // # 1: Empty, whitespace or null should be invalid
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Empty_Email_Should_Be_Invalid(string? value)
    {
        // Act
        var result = UserPropertyValidator.ValidateEmail(value!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Email should be a valid email address
    [Theory]
    [TestCase("john.doe")]
    [TestCase("john.doe@")]
    [TestCase("john.doe@com")]
    public void Invalid_Email_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateEmail(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }



}