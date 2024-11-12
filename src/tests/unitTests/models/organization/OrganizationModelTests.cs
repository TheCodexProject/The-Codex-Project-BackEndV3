using domain.models.organization;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using domain.models.workspace;

namespace unitTests.models.organization;

[TestFixture]
public class OrganizationModelTests
{
    // SECTION #1: Creation of Organization

    private readonly User _owner = User.Create("John", "Doe", "johndoe@mail.com").Value;

    // # 1: An organization should be created with a valid name and owner.
    [Test]
    public void Organization_CreateOrganizationWithValidNameAndOwner_ShouldSucceed()
    {
        // Arrange
        const string name = "Alpha";

        // Act
        var organization = Organization.Create(name, _owner);

        // Assert
        Assert.That(organization.IsSuccess, Is.True);
    }

    // # 2: An organization should not be created with an invalid name. (Too short)
    [Test]
    public void Organization_CreateOrganizationWithTooShortName_ShouldFail()
    {
        // Arrange
        const string name = "O";

        // Act
        var organization = Organization.Create(name, _owner);

        // Assert
        Assert.That(organization.IsFailure, Is.True);
    }

    // # 3: An organization should not be created with an invalid name. (Too long)
    [Test]
    public void Organization_CreateOrganizationWithTooLongName_ShouldFail()
    {
        // Arrange
        var name = "Alpha".PadLeft(101);

        // Act
        var organization = Organization.Create(name, _owner);

        // Assert
        Assert.That(organization.IsFailure, Is.True);
        Assert.That(organization.Errors.Count, Is.EqualTo(1));
    }

    // # 4: An organization should not be created with an invalid name. (Empty)
    [Test]
    public void Organization_CreateOrganizationWithEmptyName_ShouldFail()
    {
        // Arrange
        const string name = "";

        // Act
        var organization = Organization.Create(name, _owner);

        // Assert
        Assert.That(organization.IsFailure, Is.True);
    }

    // SECTION #2: Updates to Organization

    // SECTION #2.1: Update Name

    // # 1: An organization's name should be updated with a valid name.
    [Test]
    public void Organization_UpdateNameWithValidName_ShouldSucceed()
    {
        // Arrange
        const string name = "Beta";
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.UpdateName(name);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: An organization's name should not be updated with an invalid name. (Too short)
    [Test]
    public void Organization_UpdateNameWithTooShortName_ShouldFail()
    {
        // Arrange
        const string name = "O";
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.UpdateName(name);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: An organization's name should not be updated with an invalid name. (Too long)
    [Test]
    public void Organization_UpdateNameWithTooLongName_ShouldFail()
    {
        // Arrange
        var name = "Beta".PadLeft(101);
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.UpdateName(name);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 4: An organization's name should not be updated with an invalid name. (Empty)
    [Test]
    public void Organization_UpdateNameWithEmptyName_ShouldFail()
    {
        // Arrange
        const string name = "";
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.UpdateName(name);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.2: Add Member

    // # 1: An organization should be able to add a member.
    [Test]
    public void Organization_AddMember_ShouldSucceed()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var member = User.Create("Jane", "Doe", "janedoe@mail.com").Value;

        // Act
        var result = organization.AddMember(member);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(organization.Members, Has.Count.EqualTo(1));
        });
    }

    // # 2: An organization should not be able to add a member that already exists.
    [Test]
    public void Organization_AddMemberThatAlreadyExists_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var member = User.Create("Jane", "Doe", "janedoe@mail.com").Value;

        // Act
        organization.AddMember(member);
        var result = organization.AddMember(member);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: An organization should not be able to add a member that is null.
    [Test]
    public void Organization_AddNullMember_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.AddMember(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.3: Remove Member

    // # 1: An organization should be able to remove a member.
    [Test]
    public void Organization_RemoveMember_ShouldSucceed()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var member = User.Create("Jane", "Doe", "janedoe@mail.com").Value;

        // Act
        organization.AddMember(member);
        var result = organization.RemoveMember(member);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(organization.Members, Has.Count.EqualTo(0));
        });
    }

    // # 2: An organization should not be able to remove a member that does not exist.
    [Test]
    public void Organization_RemoveMemberThatDoesNotExist_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var member = User.Create("Jane", "Doe", "janedoe@mail.com").Value;

        // Act
        var result = organization.RemoveMember(member);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: An organization should not be able to remove a member that is null.
    [Test]
    public void Organization_RemoveNullMember_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.RemoveMember(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.4: Add Workspace

    // # 1: An organization should be able to add a workspace.
    [Test]
    public void Organization_AddWorkspace_ShouldSucceed()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var otherOrganization = Organization.Create("Beta", _owner).Value;
        var workspace = Workspace.Create(otherOrganization,"Alpha Workspace").Value;

        // Act
        var result = organization.AddWorkspace(workspace);

        // ! if a result is failure, log the errors
        if (result.IsFailure)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error);
            }
        }

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: An organization should not be able to add a workspace that already exists.
    [Test]
    public void Organization_AddWorkspaceThatAlreadyExists_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var workspace = Workspace.Create(organization,"Alpha Workspace").Value;

        // Act
        organization.AddWorkspace(workspace);
        var result = organization.AddWorkspace(workspace);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: An organization should not be able to add a workspace that is null.
    [Test]
    public void Organization_AddNullWorkspace_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.AddWorkspace(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.5: Remove Workspace

    // # 1: An organization should be able to remove a workspace.
    [Test]
    public void Organization_RemoveWorkspace_ShouldSucceed()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var workspace = Workspace.Create(organization,"Alpha Workspace").Value;

        // Act
        organization.AddWorkspace(workspace);
        var result = organization.RemoveWorkspace(workspace);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: An organization should not be able to remove a workspace that does not exist.
    [Test]
    public void Organization_RemoveWorkspaceThatDoesNotExist_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var otherOrganization = Organization.Create("Beta", _owner).Value;
        var workspace = Workspace.Create(otherOrganization,"Alpha Workspace").Value;

        // Act
        var result = organization.RemoveWorkspace(workspace);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: An organization should not be able to remove a workspace that is null.
    [Test]
    public void Organization_RemoveNullWorkspace_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.RemoveWorkspace(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.6: Add Resource

    // # 1: An organization should be able to add a resource.
    [Test]
    public void Organization_AddResource_ShouldSucceed()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var resource = Resource.Create("Alpha","https://www.alpha.com/v1",organization.Id,ResourceLevel.Organization).Value;

        // Act
        var result = organization.AddResource(resource);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: An organization should not be able to add a resource that already exists.
    [Test]
    public void Organization_AddResourceThatAlreadyExists_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var resource = Resource.Create("Alpha","https://www.alpha.com/v1",organization.Id,ResourceLevel.Organization).Value;

        // Act
        organization.AddResource(resource);
        var result = organization.AddResource(resource);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: An organization should not be able to add a resource that is null.
    [Test]
    public void Organization_AddNullResource_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.AddResource(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.7: Remove Resource

    // # 1: An organization should be able to remove a resource.
    [Test]
    public void Organization_RemoveResource_ShouldSucceed()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var resource = Resource.Create("Alpha","https://www.alpha.com/v1",organization.Id,ResourceLevel.Organization).Value;

        // Act
        organization.AddResource(resource);
        var result = organization.RemoveResource(resource);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: An organization should not be able to remove a resource that does not exist.
    [Test]
    public void Organization_RemoveResourceThatDoesNotExist_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;
        var resource = Resource.Create("Alpha","https://www.alpha.com/v1",organization.Id,ResourceLevel.Organization).Value;

        // Act
        var result = organization.RemoveResource(resource);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: An organization should not be able to remove a resource that is null.
    [Test]
    public void Organization_RemoveNullResource_ShouldFail()
    {
        // Arrange
        var organization = Organization.Create("Alpha", _owner).Value;

        // Act
        var result = organization.RemoveResource(null!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }
}