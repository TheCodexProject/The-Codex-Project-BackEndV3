using domain.models.organization;
using domain.models.project;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using domain.models.workspace;
using unitTests.utils;

namespace unitTests.models.workspace;

public class WorkspaceModelTests
{
    // SECTION #1: Creation of Workspace

    private readonly Organization _organization = MockDataProvider.GetOrganization();

    // # 1: An workspace should be created with a valid title and an organization.
    [Test]
    public void Workspace_Should_Be_Created_With_Valid_Title_And_Organization()
    {
        // Arrange
        const string title = "My Workspace";

        // Act
        var workspace = Workspace.Create(_organization, title);

        // Assert
        Assert.Multiple(() =>
        {

            Assert.That(workspace.IsSuccess, Is.True);
            Assert.That(workspace.Value.Title, Is.EqualTo(title));
            Assert.That(workspace.Value.Owner, Is.EqualTo(_organization));
        });
    }

    // # 2: An workspace should not be created with an invalid title. (Too short)
    [Test]
    public void Workspace_Should_Not_Be_Created_With_Invalid_Title_Too_Short()
    {
        // Arrange
        const string title = "A";

        // Act
        var workspace = Workspace.Create(_organization, title);

        // Assert
        Assert.That(workspace.IsFailure, Is.True);
    }

    // # 3: An workspace should not be created with an invalid title. (Too long)
    [Test]
    public void Workspace_Should_Not_Be_Created_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var title = "a".PadLeft(101);

        // Act
        var workspace = Workspace.Create(_organization, title);

        // Assert
        Assert.That(workspace.IsFailure, Is.True);
    }

    // # 4: An workspace should not be created with an invalid title. (Empty)
    [Test]
    public void Workspace_Should_Not_Be_Created_With_Invalid_Title_Empty()
    {
        // Arrange
        const string title = "";

        // Act
        var workspace = Workspace.Create(_organization, title);

        // Assert
        Assert.That(workspace.IsFailure, Is.True);
    }

// SECTION #2: Updates to Workspace

    // SECTION #2.1: Update title

    // # 1: An workspace title should be updated with a valid title.
    [Test]
    public void Workspace_Title_Should_Be_Updated_With_Valid_Title()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        const string newTitle = "My New Workspace";
        var result = workspace.UpdateTitle(newTitle);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workspace.Title, Is.EqualTo(newTitle));
        });
    }

    // # 2: An workspace title should not be updated with an invalid title. (Too short)
    [Test]
    public void Workspace_Title_Should_Not_Be_Updated_With_Invalid_Title_Too_Short()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        const string newTitle = "A";
        var result = workspace.UpdateTitle(newTitle);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: An workspace title should not be updated with an invalid title. (Too long)
    [Test]
    public void Workspace_Title_Should_Not_Be_Updated_With_Invalid_Title_Too_Long()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        var newTitle = "a".PadLeft(101);
        var result = workspace.UpdateTitle(newTitle);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 4: An workspace title should not be updated with an invalid title. (Empty)
    [Test]
    public void Workspace_Title_Should_Not_Be_Updated_With_Invalid_Title_Empty()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        const string newTitle = "";
        var result = workspace.UpdateTitle(newTitle);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.2: Add contact

    // # 1: A contact should be added to the workspace.
    [Test]
    public void Contact_Should_Be_Added_To_Workspace()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var contact = MockDataProvider.GetUser();

        // Act
        var result = workspace.AddContact(contact);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workspace.Contacts, Has.Member(contact));
        });
    }

    // # 2: A contact should not be added to the workspace if it already exists.
    [Test]
    public void Contact_Should_Not_Be_Added_To_Workspace_If_It_Already_Exists()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var contact = MockDataProvider.GetUser();

        // Act
        workspace.AddContact(contact);
        var result = workspace.AddContact(contact);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A contact should not be added to the workspace if it is null.
    [Test]
    public void Contact_Should_Not_Be_Added_To_Workspace_If_It_Is_Null()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        var result = workspace.AddContact(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.3: Remove contact

    // # 1: A contact should be removed from the workspace.
    [Test]
    public void Contact_Should_Be_Removed_From_Workspace()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var contact = MockDataProvider.GetUser();

        // Act
        workspace.AddContact(contact);
        var result = workspace.RemoveContact(contact);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workspace.Contacts, Has.No.Member(contact));
        });
    }

    // # 2: A contact should not be removed from the workspace if it does not exist.
    [Test]
    public void Contact_Should_Not_Be_Removed_From_Workspace_If_It_Does_Not_Exist()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var contact = MockDataProvider.GetUser();

        // Act
        var result = workspace.RemoveContact(contact);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A contact should not be removed from the workspace if it is null.
    [Test]
    public void Contact_Should_Not_Be_Removed_From_Workspace_If_It_Is_Null()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        var result = workspace.RemoveContact(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.4: Add Project

    // # 1: A project should be added to the workspace.
    [Test]
    public void Project_Should_Be_Added_To_Workspace()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var project = MockDataProvider.GetProjects(1, true)[0];

        // Act
        var result = workspace.AddProject(project);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workspace.Projects, Has.Member(project));
        });
    }

    // # 2: A project should not be added to the workspace if it already exists.
    [Test]
    public void Project_Should_Not_Be_Added_To_Workspace_If_It_Already_Exists()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var project = MockDataProvider.GetProjects(1)[0];

        // Act
        workspace.AddProject(project);
        var result = workspace.AddProject(project);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A project should not be added to the workspace if it is null.
    [Test]
    public void Project_Should_Not_Be_Added_To_Workspace_If_It_Is_Null()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        var result = workspace.AddProject(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.5: Remove Project

    // # 1: A project should be removed from the workspace.
    [Test]
    public void Project_Should_Be_Removed_From_Workspace()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var project = MockDataProvider.GetProjects(1)[0];

        // Act
        workspace.AddProject(project);
        var result = workspace.RemoveProject(project);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workspace.Projects, Has.No.Member(project));
        });
    }

    // # 2: A project should not be removed from the workspace if it does not exist.
    [Test]
    public void Project_Should_Not_Be_Removed_From_Workspace_If_It_Does_Not_Exist()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var project = MockDataProvider.GetProjects(1, true)[0];

        // Act
        var result = workspace.RemoveProject(project);

        // ! If it fails, print the error message.
        if (result.IsFailure)
            Console.WriteLine(result.Errors.First().Message);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A project should not be removed from the workspace if it is null.
    [Test]
    public void Project_Should_Not_Be_Removed_From_Workspace_If_It_Is_Null()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        var result = workspace.RemoveProject(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.6: Add Resource

    // # 1: A resource should be added to the workspace.
    [Test]
    public void Resource_Should_Be_Added_To_Workspace()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Workspace)[0];

        // Act
        var result = workspace.AddResource(resource);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workspace.Resources, Has.Member(resource));
        });
    }

    // # 2: A resource should not be added to the workspace if it already exists.
    [Test]
    public void Resource_Should_Not_Be_Added_To_Workspace_If_It_Already_Exists()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Workspace)[0];

        // Act
        workspace.AddResource(resource);
        var result = workspace.AddResource(resource);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A resource should not be added to the workspace if it is null.
    [Test]
    public void Resource_Should_Not_Be_Added_To_Workspace_If_It_Is_Null()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        var result = workspace.AddResource(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.7: Remove Resource

    // # 1: A resource should be removed from the workspace.
    [Test]
    public void Resource_Should_Be_Removed_From_Workspace()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Workspace)[0];

        // Act
        workspace.AddResource(resource);
        var result = workspace.RemoveResource(resource);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workspace.Resources, Has.No.Member(resource));
        });
    }

    // # 2: A resource should not be removed from the workspace if it does not exist.
    [Test]
    public void Resource_Should_Not_Be_Removed_From_Workspace_If_It_Does_Not_Exist()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Workspace)[0];

        // Act
        var result = workspace.RemoveResource(resource);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A resource should not be removed from the workspace if it is null.
    [Test]
    public void Resource_Should_Not_Be_Removed_From_Workspace_If_It_Is_Null()
    {
        // Arrange
        const string title = "My Workspace";
        var workspace = Workspace.Create(_organization, title).Value;

        // Act
        var result = workspace.RemoveResource(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }
}