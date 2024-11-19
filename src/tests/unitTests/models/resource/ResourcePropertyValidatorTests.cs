using domain.models.resource;
using domain.models.resource.values;

namespace unitTests.models.resource;

[TestFixture]
public class ResourcePropertyValidatorTests
{
    // SECTION #1: Validate Title

    // # 1: Empty, whitespace or null should be invalid
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Empty_Title_Should_Be_Invalid(string? value)
    {
        // Act
        var result = ResourcePropertyValidator.ValidateTitle(value!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Title with less than 3 characters should be invalid
    [Theory]
    [TestCase("a")]
    [TestCase("ab")]
    public void Title_With_Less_Than_3_Characters_Should_Be_Invalid(string value)
    {
        // Act
        var result = ResourcePropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Title with more than 75 characters should be invalid
    [Test]
    public void Title_With_More_Than_75_Characters_Should_Be_Invalid()
    {
        // Arrange
        var value = new string('a', 76);

        // Act
        var result = ResourcePropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: Title with 3 to 75 characters should be valid
    [Theory]
    [TestCase("Alpha")]
    [TestCase("Alpha Beta")]
    [TestCase("MIN")]
    public void Title_With_3_To_75_Characters_Should_Be_Valid(string value)
    {
        // Act
        var result = ResourcePropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION #2: Validate Description

    // # 1: Description with more than 500 characters should be invalid
    [Test]
    public void Description_With_More_Than_500_Characters_Should_Be_Invalid()
    {
        // Arrange
        var value = new string('a', 501);

        // Act
        var result = ResourcePropertyValidator.ValidateDescription(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Description with 500 characters or fewer should be valid
    [Theory]
    [TestCase("Alpha")]
    [TestCase("Alpha Beta")]
    [TestCase("MIN")]
    public void Description_With_500_Characters_Or_Fewer_Should_Be_Valid(string value)
    {
        // Act
        var result = ResourcePropertyValidator.ValidateDescription(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION #3: Validate URL

    // # 1: Empty, whitespace or null should be invalid
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Empty_Url_Should_Be_Invalid(string? value)
    {
        // Act
        var result = ResourcePropertyValidator.ValidateUrl(value!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: URL with a value should be valid
    [Theory]
    [TestCase("https://www.google.com")]
    [TestCase("https://www.bing.com")]
    [TestCase("https://www.duckduckgo.com")]
    public void Url_With_Value_Should_Be_Valid(string value)
    {
        // Act
        var result = ResourcePropertyValidator.ValidateUrl(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION #4: Validate Type

    // # 1: None should be invalid
    [Test]
    public void None_Type_Should_Be_Invalid()
    {
        // Arrange
        const ResourceType value = ResourceType.None;

        // Act
        var result = ResourcePropertyValidator.ValidateType(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Any other type should be valid
    [TestCase(ResourceType.Image)]
    [TestCase(ResourceType.Video)]
    [TestCase(ResourceType.Audio)]
    [TestCase(ResourceType.Document)]
    [TestCase(ResourceType.Spreadsheet)]
    [TestCase(ResourceType.Presentation)]
    [TestCase(ResourceType.Code)]
    [TestCase(ResourceType.Archive)]
    [TestCase(ResourceType.Other)]
    public void Any_Other_Type_Should_Be_Valid(ResourceType value)
    {
        // Act
        var result = ResourcePropertyValidator.ValidateType(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION #5: Validate Level

    // # 1: None should be invalid
    [Test]
    public void None_Level_Should_Be_Invalid()
    {
        // Arrange
        const ResourceLevel value = ResourceLevel.None;

        // Act
        var result = ResourcePropertyValidator.ValidateLevel(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Any other level should be valid
    [TestCase(ResourceLevel.Organization)]
    [TestCase(ResourceLevel.Workspace)]
    [TestCase(ResourceLevel.Project)]
    public void Any_Other_Level_Should_Be_Valid(ResourceLevel value)
    {
        // Act
        var result = ResourcePropertyValidator.ValidateLevel(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

}