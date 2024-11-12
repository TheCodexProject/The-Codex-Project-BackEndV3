using domain.models.organization;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using domain.models.workspace;

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

    private static List<User> GetMembers()
    {
        return
        [
            User.Create("John", "Doe", "johndoe@mail.com").Value,
            User.Create("Jane", "Doe", "janedoe@mail.com").Value
        ];
    }

    // # 1: Member is null.
    [Test]
    public void ValidateAddMember_NullMember_ShouldBeInvalid()
    {
        // Arrange
        var members = GetMembers();
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
        var members = GetMembers();
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
        var members = GetMembers();
        var member = User.Create("John", "Smith", "johnsmith@mail.com");

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
        var members = GetMembers();
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
        var members = GetMembers();
        var member = User.Create("John", "Smith", "johnsmith@mail.com").Value;

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
        var members = GetMembers();
        var member = members[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveMember(member, members);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION: Add Workspace

    private static List<Workspace> GetWorkspaces()
    {
        var owner = User.Create("John", "Doe", "johndoe@mail.com").Value;
        var org = Organization.Create("Alpha",owner).Value;

        return
        [
            Workspace.Create(org,"Alpha").Value,
            Workspace.Create(org,"Beta").Value
        ];
    }

    // # 1: Workspace is null.
    [Test]
    public void ValidateAddWorkspace_NullWorkspace_ShouldBeInvalid ()
    {
        // Arrange
        var workspaces = GetWorkspaces();
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
        var workspaces = GetWorkspaces();
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
        var workspaces = GetWorkspaces();
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
        var workspaces = GetWorkspaces();
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
        var workspaces = GetWorkspaces();
        var workspace = Workspace.Create(workspaces[0].Owner,"Gamma").Value;

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
        var workspaces = GetWorkspaces();
        var workspace = workspaces[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveWorkspace(workspace, workspaces);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // SECTION: Add Resource
    private static List<Resource> GetResources()
    {
        return
        [
            Resource.Create("Alpha","https://www.alpha.com",Guid.Empty, ResourceLevel.Organization).Value,
            Resource.Create("Beta","https://www.beta.com",Guid.Empty, ResourceLevel.Organization).Value
        ];
    }

    // # 1: Resource is null.
    [Test]
    public void ValidateAddResource_NullResource_ShouldBeInvalid()
    {
        // Arrange
        var resources = GetResources();
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
        var resources = GetResources();
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
        var resources = GetResources();
        var resource = Resource.Create("Gamma","https://www.gamma.com",Guid.Empty, ResourceLevel.Organization);

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
        var resources = GetResources();
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
        var resources = GetResources();
        var resource = Resource.Create("Gamma","https://www.gamma.com",Guid.Empty, ResourceLevel.None).Value;

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
        var resources = GetResources();
        var resource = resources[0];

        // Act
        var result = OrganizationPropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}