using domain.models.resource;
using domain.models.resource.values;
using unitTests.utils;

namespace unitTests.models.resource;

[TestFixture]
public class ResourceModelTests
{
    // SECTION #1: Creation of Resource

    // # 1: A resource can be created with a title, url, ownerId and level.
    [Test]
    public void Resource_Should_Be_Created_With_Valid_Values()
    {
        // Arrange
        const string title = "Resource Title";
        const string url = "https://www.google.com";
        var ownerId = Guid.NewGuid();
        const ResourceLevel level = ResourceLevel.Organization;

        // Act
        var result = Resource.Create(title, url, ownerId, level);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: A resource cannot be created with an invalid title. (Too short)
    [Test]
    public void Resource_Cannot_Be_Created_With_Invalid_Title_Too_Short()
    {
        // Arrange
        const string title = "A";
        const string url = "https://www.google.com";
        var ownerId = Guid.NewGuid();
        const ResourceLevel level = ResourceLevel.Organization;

        // Act
        var result = Resource.Create(title, url, ownerId, level);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A resource cannot be created with an invalid title. (Too long)
    [Test]
    public void Resource_Cannot_Be_Created_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var title = "A".PadRight(76);
        const string url = "https://www.google.com";
        var ownerId = Guid.NewGuid();
        const ResourceLevel level = ResourceLevel.Organization;

        // Act
        var result = Resource.Create(title, url, ownerId, level);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 4: A resource cannot be created with an invalid title. (Empty)
    [Test]
    public void Resource_Cannot_Be_Created_With_Invalid_Title_Empty()
    {
        // Arrange
        const string title = "";
        const string url = "https://www.google.com";
        var ownerId = Guid.NewGuid();
        const ResourceLevel level = ResourceLevel.Organization;

        // Act
        var result = Resource.Create(title, url, ownerId, level);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 5: A resource cannot be created with an invalid url. (Empty)
    [Test]
    public void Resource_Cannot_Be_Created_With_Invalid_Url_Empty()
    {
        // Arrange
        const string title = "Resource Title";
        const string url = "";
        var ownerId = Guid.NewGuid();
        const ResourceLevel level = ResourceLevel.Organization;

        // Act
        var result = Resource.Create(title, url, ownerId, level);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 6: A resource cannot be created with an invalid level. (None)
    [Test]
    public void Resource_Cannot_Be_Created_With_Invalid_Level_None()
    {
        // Arrange
        const string title = "Resource Title";
        const string url = "https://www.google.com";
        var ownerId = Guid.NewGuid();
        const ResourceLevel level = ResourceLevel.None;

        // Act
        var result = Resource.Create(title, url, ownerId, level);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2: Updates to Resource

    // SECTION #2.1: Update Title

    // # 1: A resource's title can be updated with a valid title.
    [Test]
    public void Resource_Title_Should_Be_Updated_With_Valid_Title()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const string newTitle = "New Resource Title";
        var result = resource.UpdateTitle(newTitle);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: A resource's title cannot be updated with an invalid title. (Too short)
    [Test]
    public void Resource_Title_Cannot_Be_Updated_With_Invalid_Title_Too_Short()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const string newTitle = "A";
        var result = resource.UpdateTitle(newTitle);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A resource's title cannot be updated with an invalid title. (Too long)
    [Test]
    public void Resource_Title_Cannot_Be_Updated_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        var newTitle = "A".PadRight(76);
        var result = resource.UpdateTitle(newTitle);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // # 4: A resource's title cannot be updated with an invalid title. (Empty)
    [Test]
    public void Resource_Title_Cannot_Be_Updated_With_Invalid_Title_Empty()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const string newTitle = "";
        var result = resource.UpdateTitle(newTitle);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.2: Update Description

    // # 1: A resource's description can be updated with a valid description.
    [Test]
    public void Resource_Description_Should_Be_Updated_With_Valid_Description()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const string newDescription = "New Resource Description";
        var result = resource.UpdateDescription(newDescription);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: A resource's description cannot be updated with an invalid description. (More than 500 characters)
    [Test]
    public void Resource_Description_Cannot_Be_Updated_With_Invalid_Description_Too_Long()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        var newDescription = "A".PadRight(501);
        var result = resource.UpdateDescription(newDescription);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.3: Update URL

    // # 1: A resource's url can be updated with a valid url.
    [Test]
    public void Resource_Url_Should_Be_Updated_With_Valid_Url()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const string newUrl = "https://www.newurl.com";
        var result = resource.UpdateUrl(newUrl);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: A resource's url cannot be updated with an invalid url. (Empty)
    [Test]
    public void Resource_Url_Cannot_Be_Updated_With_Invalid_Url_Empty()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const string newUrl = "";
        var result = resource.UpdateUrl(newUrl);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.4: Update Type

    // # 1: A resource's type can be updated with a valid type.
    [Test]
    public void Resource_Type_Should_Be_Updated_With_Valid_Type()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const ResourceType newType = ResourceType.Image;
        var result = resource.UpdateType(newType);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: A resource's type cannot be updated with an invalid type. (None)
    [Test]
    public void Resource_Type_Cannot_Be_Updated_With_Invalid_Type_None()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const ResourceType newType = ResourceType.None;
        var result = resource.UpdateType(newType);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.5: Update Level

    // # 1: A resource's level can be updated with a valid level.
    [Test]
    public void Resource_Level_Should_Be_Updated_With_Valid_Level()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const ResourceLevel newLevel = ResourceLevel.Workspace;
        var result = resource.UpdateLevel(newLevel);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }

    // # 2: A resource's level cannot be updated with an invalid level. (None)
    [Test]
    public void Resource_Level_Cannot_Be_Updated_With_Invalid_Level_None()
    {
        // Arrange
        var resource = MockDataProvider.GetResources(1, ResourceLevel.Organization)[0];

        // Act
        const ResourceLevel newLevel = ResourceLevel.None;
        var result = resource.UpdateLevel(newLevel);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }
}