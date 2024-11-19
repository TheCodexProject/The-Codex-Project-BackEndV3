using domain.models.organization;
using domain.models.user;
using unitTests.utils;

namespace unitTests.models.user;

[TestFixture]
public class UserModelTests
{
    // SECTION #1: Creation of User

    // # 1: A user should be created with a valid first name, last name, and email
    [Test]
    public void User_With_Valid_FirstName_LastName_And_Email_Should_Be_Created()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "johndoe@mail.com";

        // Act
        var result = User.Create(firstName, lastName, email);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value.FirstName, Is.EqualTo(firstName));
    }

    // # 2: A user should not be created with an invalid first name
    [Test]
    public void User_With_Invalid_FirstName_Should_Not_Be_Created()
    {
        // Arrange
        const string firstName = "John1";
        const string lastName = "Doe";
        const string email = "johndoe@mail.com";

        // Act
        var result = User.Create(firstName, lastName, email);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: A user should not be created with an invalid last name
    [Test]
    public void User_With_Invalid_LastName_Should_Not_Be_Created()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe1";
        const string email = "johndoe@mail.com";

        // Act
        var result = User.Create(firstName, lastName, email);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: A user should not be created with an invalid email
    [Test]
    public void User_With_Invalid_Email_Should_Not_Be_Created()
    {
        // Arrange
        const string firstName = "John";
        const string lastName = "Doe";
        const string email = "johndoeemail.com";

        // Act
        var result = User.Create(firstName, lastName, email);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 5: A user should not be created with an invalid first name, last name, and email
    [Test]
    public void User_With_Invalid_FirstName_LastName_And_Email_Should_Not_Be_Created()
    {
        // Arrange
        const string firstName = "John1";
        const string lastName = "Doe1";
        const string email = "johndoeemail.com";

        // Act
        var result = User.Create(firstName, lastName, email);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(3));
    }

    // # 6: A user should not be created with an invalid first name and last name
    [Test]
    public void User_With_Invalid_FirstName_And_LastName_Should_Not_Be_Created()
    {
        // Arrange
        const string firstName = "John1";
        const string lastName = "Doe1";
        const string email = "johndoe@mail.com";

        // Act
        var result = User.Create(firstName, lastName, email);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(2));
    }

    // # 7: A user should not be created with an invalid first name and email
    [Test]
    public void User_With_Invalid_FirstName_And_Email_Should_Not_Be_Created()
    {
        // Arrange
        const string firstName = "John1";
        const string lastName = "Doe";
        const string email = "johndoeemail.com";

        // Act
        var result = User.Create(firstName, lastName, email);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(2));
    }

    // SECTION #2: Updates to User

    // SECTION #2.1: Update First Name

    // # 1: A user's first name should be updated with a valid first name
    [Test]
    public void User_FirstName_Should_Be_Updated_With_Valid_FirstName()
    {
        // Arrange
        const string firstName = "Bob";

        var user = MockDataProvider.GetUser();

        // Act
        var result = user.UpdateFirstName(firstName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.FirstName, Is.EqualTo(firstName));
        });
    }

    // # 2: A user's first name should not be updated with an invalid first name
    [Test]
    public void User_FirstName_Should_Not_Be_Updated_With_Invalid_FirstName()
    {
        // Arrange
        const string firstName = "Bob1";

        var user = MockDataProvider.GetUser();

        // Act
        var result = user.UpdateFirstName(firstName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(user.FirstName, Is.Not.EqualTo(firstName));
        });
    }

    // SECTION #2.2: Update Last Name

    // # 1: A user's last name should be updated with a valid last name
    [Test]
    public void User_LastName_Should_Be_Updated_With_Valid_LastName()
    {
        // Arrange
        const string lastName = "Smith";

        var user = MockDataProvider.GetUser();

        // Act
        var result = user.UpdateLastName(lastName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.LastName, Is.EqualTo(lastName));
        });
    }

    // # 2: A user's last name should not be updated with an invalid last name
    [Test]
    public void User_LastName_Should_Not_Be_Updated_With_Invalid_LastName()
    {
        // Arrange
        const string lastName = "Smith1";

        var user = MockDataProvider.GetUser();

        // Act
        var result = user.UpdateLastName(lastName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(user.LastName, Is.Not.EqualTo(lastName));
        });
    }

    // SECTION #2.3: Update Email

    // # 1: A user's email should be updated with a valid email
    [Test]
    public void User_Email_Should_Be_Updated_With_Valid_Email()
    {
        // Arrange
        const string email = "johnsmith@mail.com";

        var user = MockDataProvider.GetUser();

        // Act
        var result = user.UpdateEmail(email);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.Email, Is.EqualTo(email));
        });
    }

    // # 2: A user's email should not be updated with an invalid email
    [Test]
    public void User_Email_Should_Not_Be_Updated_With_Invalid_Email()
    {
        // Arrange
        const string email = "johnsmithmail.com";

        var user = MockDataProvider.GetUser();

        // Act
        var result = user.UpdateEmail(email);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(user.Email, Is.Not.EqualTo(email));
        });
    }

    // SECTION #2.4: Join Organization

    // # 1: A user should be able to join an organization
    [Test]
    public void User_Should_Be_Able_To_Join_Organization()
    {
        // Arrange
        var user = User.Create("John", "Doe", "johndoe@mail.com");
        var organizations = MockDataProvider.GetOrganizations(2);

        // Act
        var result = user.Value.JoinOrganization(organizations[0]);
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.Value.Memberships, Has.Count.EqualTo(1));
        });
    }

    // # 2: A user should not be able to join an organization if they are already a member
    [Test]
    public void User_Should_Not_Be_Able_To_Join_Organization_If_Already_A_Member()
    {
        // Arrange
        var user = User.Create("John", "Doe", "johndoe@mail.com");
        var organizations = MockDataProvider.GetOrganizations(2);

        // Act
        user.Value.JoinOrganization(organizations[0]);
        var result = user.Value.JoinOrganization(organizations[0]);
        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(user.Value.Memberships, Has.Count.EqualTo(1));
        });
    }

    // # 3: A user should not be able to join an organization if the organization is null
    [Test]
    public void User_Should_Not_Be_Able_To_Join_Organization_If_Organization_Is_Null()
    {
        // Arrange
        var user = User.Create("John", "Doe", "johndoe@mail.com");

        // Act
        var result = user.Value.JoinOrganization(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.5: Leave Organization

    // # 1: A user should be able to leave an organization
    [Test]
    public void User_Should_Be_Able_To_Leave_Organization()
    {
        // Arrange
        var user = User.Create("John", "Doe", "johndoe@mail.com");
        var organizations = MockDataProvider.GetOrganizations(2);

        // Act
        user.Value.JoinOrganization(organizations[0]);
        var result = user.Value.LeaveOrganization(organizations[0]);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.Value.Memberships, Has.Count.EqualTo(0));
        });
    }

    // # 2: A user should not be able to leave an organization if they are not a member
    [Test]
    public void User_Should_Not_Be_Able_To_Leave_Organization_If_Not_A_Member()
    {
        // Arrange
        var user = User.Create("John", "Doe", "johndoe@mail.com");
        var organizations = MockDataProvider.GetOrganizations(2);

        // Act
        var result = user.Value.LeaveOrganization(organizations[0]);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A user should not be able to leave an organization if the organization is null
    [Test]
    public void User_Should_Not_Be_Able_To_Leave_Organization_If_Organization_Is_Null()
    {
        // Arrange
        var user = User.Create("John", "Doe", "johndoe@mail.com");

        // Act
        var result = user.Value.LeaveOrganization(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }
}