using domain.models.organization;
using domain.models.project;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using domain.models.workspace;
using unitTests.utils;

namespace unitTests.models.workspace;

public class WorkspacePropertyValidatorTests
{
    // SECTION #1: Validate Title

    // # 1: Empty, whitespace, or null should be invalid.
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Empty_Title_Should_Be_Invalid(string? value)
    {
        // Act
        var result = WorkspacePropertyValidator.ValidateTitle(value!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Title should be at least 3 characters.
    [Theory]
    [TestCase("a")]
    [TestCase("ab")]
    public void Title_Should_Be_At_Least_3_Characters(string value)
    {
        // Act
        var result = WorkspacePropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Title should be at most 100 characters.
    [Test]
    public void Title_Should_Be_At_Most_100_Characters()
    {
        // Arrange
        var value = new string('a', 101);

        // Act
        var result = WorkspacePropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: Title should be valid.
    [Theory]
    [TestCase("Alpha Bravo Charlie")]
    [TestCase("Alpha")]
    [TestCase("MIN")]
    public void Title_Should_Be_Valid(string value)
    {
        // Act
        var result = WorkspacePropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }


    // SECTION #2: Validate Add Contact
    // # 1: Contact cannot be null.
    [Test]
    public void Contact_Should_Not_Be_Null()
    {
        // Arrange
        User? contact = null;
        var contacts = MockDataProvider.GetUsers(3);

        // Act
        var result = WorkspacePropertyValidator.ValidateAddContact(contact, contacts);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Contact is already in the list.
    [Test]
    public void Contact_Should_Already_Exist_In_List()
    {
        // Arrange
        var contacts = MockDataProvider.GetUsers(3);
        var contact = contacts[0];

        // Act
        var result = WorkspacePropertyValidator.ValidateAddContact(contact, contacts);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Contact is not already in the list.
    [Test]
    public void Contact_Should_Not_Already_Exist_In_List()
    {
        // Arrange
        var contacts = MockDataProvider.GetUsers(3);
        var contact = User.Create("John", "Smith", "johnsmith@mail.com").Value;

        // Act
        var result = WorkspacePropertyValidator.ValidateAddContact(contact, contacts);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #3: Validate Remove Contact

    // # 1: Contact cannot be null.
    [Test]
    public void Contact_Should_Not_Be_Null_For_Removal()
    {
        // Arrange
        User? contact = null;
        var contacts = MockDataProvider.GetUsers(3);

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveContact(contact, contacts);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Contact is not in the list.
    [Test]
    public void Contact_Should_Not_Exist_In_List_For_Removal()
    {
        // Arrange
        var contacts = MockDataProvider.GetUsers(3);
        var contact = User.Create("John", "Smith", "johnsmith@mail.com").Value;

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveContact(contact, contacts);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Contact is in the list.
    [Test]
    public void Contact_Should_Exist_In_List_For_Removal()
    {
        // Arrange
        var contacts = MockDataProvider.GetUsers(3);
        var contact = contacts[0];

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveContact(contact, contacts);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #4: Validate Add Project

    // # 1: Project cannot be null.
    [Test]
    public void Project_Should_Not_Be_Null()
    {
        // Arrange
        var projects = MockDataProvider.GetProjects(3);
        Project? project = null;

        // Act
        var result = WorkspacePropertyValidator.ValidateAddProject(project, projects);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Project is already in the list.
    [Test]
    public void Project_Should_Already_Exist_In_List()
    {
        // Arrange
        var projects = MockDataProvider.GetProjects(3);
        var project = projects[0];

        // Act
        var result = WorkspacePropertyValidator.ValidateAddProject(project, projects);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Project is not already in the list.
    [Test]
    public void Project_Should_Not_Already_Exist_In_List()
    {
        // Arrange
        var projects = MockDataProvider.GetProjects(3);
        var project = Project.Create(projects[0].Workspace, "Project 3").Value;

        // Act
        var result = WorkspacePropertyValidator.ValidateAddProject(project, projects);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #5: Validate Remove Project

    // # 1: Project cannot be null.
    [Test]
    public void Project_Should_Not_Be_Null_For_Removal()
    {
        // Arrange
        var projects = MockDataProvider.GetProjects(3);
        Project? project = null;

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveProject(project, projects);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Project is not in the list.
    [Test]
    public void Project_Should_Not_Exist_In_List_For_Removal()
    {
        // Arrange
        var projects = MockDataProvider.GetProjects(3);
        var project = Project.Create(projects[0].Workspace, "Project 3").Value;

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveProject(project, projects);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Project is in the list.
    [Test]
    public void Project_Should_Exist_In_List_For_Removal()
    {
        // Arrange
        var projects = MockDataProvider.GetProjects(3);
        var project = projects[0];

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveProject(project, projects);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #6: Validate Add Resource

    // # 1: Resource cannot be null.
    [Test]
    public void Resource_Should_Not_Be_Null()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3, ResourceLevel.Workspace);
        Resource? resource = null;

        // Act
        var result = WorkspacePropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Resource is already in the list.
    [Test]
    public void Resource_Should_Already_Exist_In_List()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3, ResourceLevel.Workspace);
        var resource = resources[0];

        // Act
        var result = WorkspacePropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Resource is not already in the list.
    [Test]
    public void Resource_Should_Not_Already_Exist_In_List()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3, ResourceLevel.Workspace);
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Workspace)[0];

        // Act
        var result = WorkspacePropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #7: Validate Remove Resource

    // # 1: Resource cannot be null.
    [Test]
    public void Resource_Should_Not_Be_Null_For_Removal()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3, ResourceLevel.Workspace);
        Resource? resource = null;

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Resource is not in the list.
    [Test]
    public void Resource_Should_Not_Exist_In_List_For_Removal()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3, ResourceLevel.Workspace);
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Workspace)[0];

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Resource is in the list.
    [Test]
    public void Resource_Should_Exist_In_List_For_Removal()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3, ResourceLevel.Workspace);
        var resource = resources[0];

        // Act
        var result = WorkspacePropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }
}