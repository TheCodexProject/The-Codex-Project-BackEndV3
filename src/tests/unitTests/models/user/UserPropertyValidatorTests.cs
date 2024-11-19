using domain.models.organization;
using domain.models.user;
using unitTests.utils;

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
    [TestCase("invalid email")]
    [TestCase("invalid.email")]
    [TestCase("invalid@mail")]
    [TestCase("invalid@mail.")]
    [TestCase("invalid@mail.c")]
    [TestCase("invalid@mail.c.")]
    public void Invalid_Email_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateEmail(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Email cannot contain any special characters or have missing parts
    [Theory]
    [TestCase("missing_at_symbol.com")] // Missing @ symbol
    [TestCase("@domain.com")] // Empty local part
    [TestCase("localPart@")] // Empty domain part
    [TestCase("local@part@domain.com")] // Multiple @ symbols
    [TestCase("special&character@domain.com")] // Invalid special character in local part
    [TestCase("localpart@dom#ain.com")] // Special character in domain part
    [TestCase("space in a local part@domain.com")]
    public void Email_With_Invalid_Parts_Should_Be_Invalid(string value)
    {
        // Act
        var result = UserPropertyValidator.ValidateEmail(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: Email cannot exceed length limits (min 5, max 254)
    [Theory]
    [TestCase("this_is_way_too_long_for_gmail_to_accept_as_a_local_part_of_an_email@domain.com",
        true)] // Local part too long
    [TestCase("validlocalpart@domainwithaveryveryveryveryveryveryveryveryveryveryverylongsegment.com",
        true)] // Domain part too long
    [TestCase("valid.email@domain.com", false)] // Valid email
    [TestCase("a@b.com", false)] // Short but valid email
    public void Email_With_Invalid_Length_Should_Be_Invalid(string value, bool expected)
    {
        // Act
        var result = UserPropertyValidator.ValidateEmail(value);

        // Assert
        Assert.That(result.IsFailure, Is.EqualTo(expected));
        Assert.That(result.Errors.Count, Is.EqualTo(expected ? 1 : 0));
    }

    // # 5: Email cannot have wrong dot (.) placement
    [Theory]
    [TestCase("local..part@domain.com", true)] // Consecutive dots in local part
    [TestCase("localpart@domain..com", true)] // Consecutive dots in domain part
    [TestCase(".localpart@domain.com", true)] // Local part starts with a dot
    [TestCase("localPart.@domain.com", true)] // Local part ends with a dot
    [TestCase("valid.email@domain.com", false)] // Valid email with proper dots
    public void Email_With_Invalid_Dot_Placement_Should_Be_Invalid(string value, bool expected)
    {
        // Act
        var result = UserPropertyValidator.ValidateEmail(value);

        // Assert
        Assert.That(result.IsFailure, Is.EqualTo(expected));
        Assert.That(result.Errors.Count, Is.EqualTo(expected ? 1 : 0));
    }

    // # 6: Email needs a domain that is a valid TLD
    [Theory]
    [TestCase("localpart@domain.c", true)] // TLD too short
    [TestCase("localpart@domain.com", false)] // Valid TLD
    [TestCase("localpart@short.co", false)] // Valid short TLD
    public void Email_With_Invalid_TLD_Should_Be_Invalid(string value, bool expected)
    {
        // Act
        var result = UserPropertyValidator.ValidateEmail(value);

        // Assert
        Assert.That(result.IsFailure, Is.EqualTo(expected));
        Assert.That(result.Errors.Count, Is.EqualTo(expected ? 1 : 0));
    }

    // SECTION #4: Add Organization

    // # 1: Organization is null.
    [Test]
    public void ValidateAddOrganization_NullOrganization_ShouldBeInvalid()
    {
        // Arrange
        var organizations = MockDataProvider.GetOrganizations(3);

        // Act
        var result = UserPropertyValidator.ValidateAddOrganization(null, organizations);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Organization is already in the list.
    [Test]
    public void ValidateAddOrganization_OrganizationAlreadyExists_ShouldBeInvalid()
    {
        // Arrange
        var organizations = MockDataProvider.GetOrganizations(3);
        var organization = organizations[0];

        // Act
        var result = UserPropertyValidator.ValidateAddOrganization(organization, organizations);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Organization is not in the list.
    [Test]
    public void ValidateAddOrganization_AllowsAdditionOfNewOrganization()
    {
        // Arrange
        var organizations = MockDataProvider.GetOrganizations(3);
        var organization = MockDataProvider.GetOrganizations(1)[0];

        // Act
        var result = UserPropertyValidator.ValidateAddOrganization(organization, organizations);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION #5: Remove Organization

    // # 1: Organization is null.
    [Test]
    public void ValidateRemoveOrganization_NullOrganization_ShouldBeInvalid()
    {
        // Arrange
        var organizations = MockDataProvider.GetOrganizations(3);

        // Act
        var result = UserPropertyValidator.ValidateRemoveOrganization(null, organizations);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Organization is not in the list.
    [Test]
    public void ValidateRemoveOrganization_OrganizationDoesNotExist_ShouldBeInvalid()
    {
        // Arrange
        var organizations = MockDataProvider.GetOrganizations(3);
        var organization = MockDataProvider.GetOrganizations(1)[0];

        // Act
        var result = UserPropertyValidator.ValidateRemoveOrganization(organization, organizations);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Organization is in the list.
    [Test]
    public void ValidateRemoveOrganization_AllowsDeletionOfExistingOrganization_Success()
    {
        // Arrange
        var organizations = MockDataProvider.GetOrganizations(3);
        var organization = organizations[0];

        // Act
        var result = UserPropertyValidator.ValidateRemoveOrganization(organization, organizations);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}