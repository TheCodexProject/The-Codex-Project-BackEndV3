using domain.models.user;

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

        var user = User.Create("John", "Doe", "johndoe@mail.com");

        // Act
        var result = user.Value.UpdateFirstName(firstName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.Value.FirstName, Is.EqualTo(firstName));
        });
    }

    // # 2: A user's first name should not be updated with an invalid first name
    [Test]
    public void User_FirstName_Should_Not_Be_Updated_With_Invalid_FirstName()
    {
        // Arrange
        const string firstName = "Bob1";

        var user = User.Create("John", "Doe", "johndoe@mail.com");

        // Act
        var result = user.Value.UpdateFirstName(firstName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(user.Value.FirstName, Is.Not.EqualTo(firstName));
        });
    }

    // SECTION #2.2: Update Last Name

    // # 1: A user's last name should be updated with a valid last name
    [Test]
    public void User_LastName_Should_Be_Updated_With_Valid_LastName()
    {
        // Arrange
        const string lastName = "Smith";

        var user = User.Create("John", "Doe", "johnsmith@mail.com");

        // Act
        var result = user.Value.UpdateLastName(lastName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.Value.LastName, Is.EqualTo(lastName));
        });
    }

    // # 2: A user's last name should not be updated with an invalid last name
    [Test]
    public void User_LastName_Should_Not_Be_Updated_With_Invalid_LastName()
    {
        // Arrange
        const string lastName = "Smith1";

        var user = User.Create("John", "Doe", "johndoe@mail.com");

        // Act
        var result = user.Value.UpdateLastName(lastName);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(user.Value.LastName, Is.Not.EqualTo(lastName));
        });
    }

    // SECTION #2.3: Update Email

    // # 1: A user's email should be updated with a valid email
    [Test]
    public void User_Email_Should_Be_Updated_With_Valid_Email()
    {
        // Arrange
        const string email = "johnsmith@mail.com";

        var user = User.Create("John", "Doe", "johndoe@mail.com");

        // Act
        var result = user.Value.UpdateEmail(email);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(user.Value.Email, Is.EqualTo(email));
        });
    }

    // # 2: A user's email should not be updated with an invalid email
    [Test]
    public void User_Email_Should_Not_Be_Updated_With_Invalid_Email()
    {
        // Arrange
        const string email = "johnsmithmail.com";

        var user = User.Create("John", "Doe", "johndoe@mail.com");

        // Act
        var result = user.Value.UpdateEmail(email);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(user.Value.Email, Is.Not.EqualTo(email));
        });
    }
}