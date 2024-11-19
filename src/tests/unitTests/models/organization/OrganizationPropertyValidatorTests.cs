using domain.models.organization;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using domain.models.workspace;
using unitTests.utils;

namespace unitTests.models.organization;

[TestFixture]
public class OrganizationPropertyValidatorTests
{
    // SECTION: Name

    // # 1: Empty, whitespace, or null name.
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void ValidateName_InvalidName_ThrowsException(string? value)
    {
        // Act
        var result = OrganizationPropertyValidator.ValidateName(value);

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 2: Name is too short. (Less than 2 characters)
    [Test]
    public void ValidateName_NameTooShort_ThrowsException()
    {
        // Arrange
        const string name = "A";

        // Act
        var result = OrganizationPropertyValidator.ValidateName(name);

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: Name is too long. (More than 100 characters)
    [Test]
    public void ValidateName_NameTooLong_ThrowsException()
    {
        // Arrange
        var name = "Alpha".PadLeft(101);
        // Act
        var result = OrganizationPropertyValidator.ValidateName(name);

        // Assert
        Assert.Multiple(()=>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 4: Name is valid.
    [Theory]
    [TestCase("Alpha")]
    [TestCase("Alpha Inc.")]
    [TestCase("Alpha Aps.")]
    [TestCase("Alpha A/S")]
    public void ValidateName_ValidName_ReturnsName(string name)
    {
        // Act
        var result = OrganizationPropertyValidator.ValidateName(name);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(name));
        });
    }

    // SECTION: Add Member

    // # 1: Member is null.
    [Test]
    public void ValidateAddMember_NullMember_ShouldBeInvalid()
    {
        // Arrange
        var members = MockDataProvider.GetUsers(3);
        User? member = null;

        // Act
        var result = OrganizationPropertyValidator.ValidateAddMember(member, members);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 2: Member already exists in the list.
    [Test]
    public void ValidateAddMember_MemberAlreadyExists_ShouldBeInvalid()
    {
        // Arrange
        var members = MockDataProvider.GetUsers(3);
        var member = members[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateAddMember(member, members);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: Member does not exist in the list.
    [Test]
    public void ValidateAddMember_AllowsAdditionOfNewMember()
    {
        // Arrange
        var members = MockDataProvider.GetUsers(3);
        var member = MockDataProvider.GetUser();

        // Act
        var result = OrganizationPropertyValidator.ValidateAddMember(member, members);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION: Remove Member

    // # 1: Member is null.
    [Test]
    public void ValidateRemoveMember_NullMember_ShouldBeInvalid()
    {
        // Arrange
        var members = MockDataProvider.GetUsers(3);
        User? member = null;

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveMember(member, members);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 2: Member does not exist in the list.
    [Test]
    public void ValidateRemoveMember_MemberDoesNotExist_ShouldBeInvalid()
    {
        // Arrange
        var members = MockDataProvider.GetUsers(3);
        var member = MockDataProvider.GetUser();

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveMember(member, members);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: Member exists in the list.
    [Test]
    public void ValidateRemoveMember_AllowsDeletionOfExistingMember_Success()
    {
        // Arrange
        var members = MockDataProvider.GetUsers(3);
        var member = members[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveMember(member, members);

        // ! Print failure message.
        if (result.IsFailure)
        {
            Console.WriteLine(result.Errors.First().Message);
        }

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION: Add Workspace

    // # 1: Workspace is null.
    [Test]
    public void ValidateAddWorkspace_NullWorkspace_ShouldBeInvalid ()
    {
        // Arrange
        var workspaces = MockDataProvider.GetWorkspaces(2);
        Workspace? workspace = null;

        // Act
        var result = OrganizationPropertyValidator.ValidateAddWorkspace(workspace, workspaces);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 2: Workspace already exists in the list.
    [Test]
    public void ValidateAddWorkspace_WorkspaceAlreadyExists_ShouldBeInvalid()
    {
        // Arrange
        var workspaces = MockDataProvider.GetWorkspaces(2);
        var workspace = workspaces[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateAddWorkspace(workspace, workspaces);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: Workspace does not exist in the list.
    [Test]
    public void ValidateAddWorkspace_AllowsAdditionOfNewWorkspace()
    {
        // Arrange
        var workspaces = MockDataProvider.GetWorkspaces(2);
        var workspace = Workspace.Create(workspaces[0].Owner,"Gamma").Value;

        // Act
        var result = OrganizationPropertyValidator.ValidateAddWorkspace(workspace, workspaces);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION: Remove Workspace

    // # 1: Workspace is null.
    [Test]
    public void ValidateRemoveWorkspace_NullWorkspace_ShouldBeInvalid()
    {
        // Arrange
        var workspaces = MockDataProvider.GetWorkspaces(2);
        Workspace? workspace = null;

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveWorkspace(workspace, workspaces);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 2: Workspace does not exist in the list.
    [Test]
    public void ValidateRemoveWorkspace_WorkspaceDoesNotExist_ShouldBeInvalid()
    {
        // Arrange
        var workspaces = MockDataProvider.GetWorkspaces(2);
        var workspace = MockDataProvider.GetWorkspace();

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveWorkspace(workspace, workspaces);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: Workspace exists in the list.
    [Test]
    public void ValidateRemoveWorkspace_AllowsDeletionOfExistingWorkspace_Success()
    {
        // Arrange
        var workspaces = MockDataProvider.GetWorkspaces(2);
        var workspace = workspaces[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveWorkspace(workspace, workspaces);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION: Add Resource


    // # 1: Resource is null.
    [Test]
    public void ValidateAddResource_NullResource_ShouldBeInvalid()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(2, ResourceLevel.Organization);
        Resource? resource = null;

        // Act
        var result = OrganizationPropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 2: Resource already exists in the list.
    [Test]
    public void ValidateAddResource_ResourceAlreadyExists_ShouldBeInvalid()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(2, ResourceLevel.Organization);
        var resource = resources[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: Resource does not exist in the list.
    [Test]
    public void ValidateAddResource_AllowsAdditionOfNewResource()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(2, ResourceLevel.Organization);
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION: Remove Resource

    // # 1: Resource is null.
    [Test]
    public void ValidateRemoveResource_NullResource_ShouldBeInvalid()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(2, ResourceLevel.Organization);
        Resource? resource = null;

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 2: Resource does not exist in the list.
    [Test]
    public void ValidateRemoveResource_ResourceDoesNotExist_ShouldBeInvalid()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(2, ResourceLevel.Organization);
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: Resource exists in the list.
    [Test]
    public void ValidateRemoveResource_AllowsDeletionOfExistingResource_Success()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(2, ResourceLevel.Organization);
        var resource = resources[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}