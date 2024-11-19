using domain.models.project;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using domain.models.workItem;
using unitTests.utils;

namespace unitTests.models.projectActivity;

[TestFixture]
public class ProjectActivityModelTests
{
    // SECTION #1: Creation of ProjectActivity

    // # 1: A project activity should be with a valid title and type.
    [TestCase(ProjectActivityType.Milestone)]
    [TestCase(ProjectActivityType.Iteration)]
    public void Activity_Should_Be_Created_With_Valid_Values(ProjectActivityType activityType)
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "Test Milestone";
        // Act
        var projectActivity = ProjectActivity.Create(project, title, activityType);

        // Assert
        Assert.That(projectActivity.IsSuccess, Is.True);
    }

    // # 2: A project activity should not be created with an empty title.
    [Test]
    public void Activity_Should_Not_Be_Created_With_Invalid_Title_Empty()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "";

        // Act
        var projectActivity = ProjectActivity.Create(project, title, ProjectActivityType.Milestone);

        // Assert
        Assert.That(projectActivity.IsFailure, Is.True);
    }

    // # 3: A project activity should not be created with a title that is too short.
    [Test]
    public void Activity_Should_Not_Be_Created_With_Invalid_Title_Too_Short()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "T";

        // Act
        var projectActivity = ProjectActivity.Create(project, title, ProjectActivityType.Milestone);

        // Assert
        Assert.That(projectActivity.IsFailure, Is.True);
    }

    // # 4: A project activity should not be created with a title that is too long.
    [Test]
    public void Activity_Should_Not_Be_Created_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "This is a very long title that is over 75 characters long and should not be allowed to be created.";

        // Act
        var projectActivity = ProjectActivity.Create(project, title, ProjectActivityType.Milestone);

        // Assert
        Assert.That(projectActivity.IsFailure, Is.True);
    }

    // # 5: A project activity should not be created with an empty type.
    [Test]
    public void Activity_Should_Not_Be_Created_With_Invalid_Type_None()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "Test Milestone";

        // Act
        var projectActivity = ProjectActivity.Create(project, title, ProjectActivityType.None);

        // Assert
        Assert.That(projectActivity.IsFailure, Is.True);
    }

    // SECTION #2: Update of ProjectActivity

    // SECTION #2.1: Update Title

    // # 1: A project activity should be updated with a valid title.
    [Test]
    public void Activity_Title_Should_Be_Updated_With_Valid_Title()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        const string title = "Updated Milestone";

        // Act
        var updatedProjectActivity = projectActivity.UpdateTitle(title);

        // Assert
        Assert.That(updatedProjectActivity.IsSuccess, Is.True);
    }

    // # 2: A project activity should not be updated with an empty title.
    [Test]
    public void Activity_Title_Should_Not_Be_Updated_With_Invalid_Title_Empty()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        const string title = "";

        // Act
        var updatedProjectActivity = projectActivity.UpdateTitle(title);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }

    // # 3: A project activity should not be updated with a title that is too short.
    [Test]
    public void Activity_Title_Should_Not_Be_Updated_With_Invalid_Title_Too_Short()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        const string title = "T";

        // Act
        var updatedProjectActivity = projectActivity.UpdateTitle(title);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }

    // # 4: A project activity should not be updated with a title that is too long.
    [Test]
    public void Activity_Title_Should_Not_Be_Updated_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        const string title = "This is a very long title that is over 75 characters long and should not be allowed to be updated.";

        // Act
        var updatedProjectActivity = projectActivity.UpdateTitle(title);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }

    // SECTION #2.2: Update Description

    // # 1: A project activity should be updated with a valid description.
    [Test]
    public void Activity_Description_Should_Be_Updated_With_Valid_Description()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        const string description = "This is a test description for the project activity.";

        // Act
        var updatedProjectActivity = projectActivity.UpdateDescription(description);

        // Assert
        Assert.That(updatedProjectActivity.IsSuccess, Is.True);
    }

    // # 2: A project activity should not be updated with a description that is too long. (More than 500 characters)
    [Test]
    public void Activity_Description_Should_Not_Be_Updated_With_Invalid_Description_Too_Long()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        const string description = "This is a very long description that is over 500 characters long and should not be allowed to be updated. This is a very long description that is over 500 characters long and should not be allowed to be updated. This is a very long description that is over 500 characters long and should not be allowed to be updated. This is a very long description that is over 500 characters long and should not be allowed to be updated. This is a very long description that is over 500 characters long and should not be allowed to be updated. This is a very long description that is over 500 characters long and should not be allowed to be updated. This is a very long description that is over 500 characters long and should not be allowed to be updated. This is a very long description that is over 500 characters long and should not be allowed to be updated.";

        // Act
        var updatedProjectActivity = projectActivity.UpdateDescription(description);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }

    // SECTION #2.3: Update Type

    // # 1: A project activity should be updated with a valid type.
    [Test]
    public void Activity_Type_Should_Be_Updated_With_Valid_Type()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        var type = ProjectActivityType.Iteration;

        // Act
        var updatedProjectActivity = projectActivity.UpdateType(type);

        // Assert
        Assert.That(updatedProjectActivity.IsSuccess, Is.True);
    }

    // # 2: A project activity should not be updated with an empty type. (None)
    [Test]
    public void Activity_Type_Should_Not_Be_Updated_With_Invalid_Type_None()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        var type = ProjectActivityType.None;

        // Act
        var updatedProjectActivity = projectActivity.UpdateType(type);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }

    // SECTION #2.4: Update Add WorkItem

    // # 1: A work item should be added to the project activity.
    [Test]
    public void WorkItem_Should_Be_Added_To_Activity()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        var workItem = MockDataProvider.GetTasks(1)[0];

        // Act
        var updatedProjectActivity = projectActivity.AddWorkItem(workItem);

        // Assert
        Assert.That(updatedProjectActivity.IsSuccess, Is.True);
    }

    // # 2: A work item should not be added to the project activity if it already exists.
    [Test]
    public void WorkItem_Should_Not_Be_Added_To_Activity_If_It_Already_Exists()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        var workItem = MockDataProvider.GetTasks(1)[0];
        projectActivity.AddWorkItem(workItem);

        // Act
        var updatedProjectActivity = projectActivity.AddWorkItem(workItem);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }

    // # 3: A work item should not be added to the project activity if it is null.
    [Test]
    public void WorkItem_Should_Not_Be_Added_To_Activity_If_It_Is_Null()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        WorkItem? workItem = null;

        // Act
        var updatedProjectActivity = projectActivity.AddWorkItem(workItem!);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }

    // SECTION #2.5: Update Remove WorkItem

    // # 1: A work item should be removed from the project activity.
    [Test]
    public void WorkItem_Should_Be_Removed_From_Activity()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        var workItem = MockDataProvider.GetTasks(1)[0];
        projectActivity.AddWorkItem(workItem);

        // Act
        var updatedProjectActivity = projectActivity.RemoveWorkItem(workItem);

        // Assert
        Assert.That(updatedProjectActivity.IsSuccess, Is.True);
    }

    // # 2: A work item should not be removed from the project activity if it does not exist.
    [Test]
    public void WorkItem_Should_Not_Be_Removed_From_Activity_If_It_Does_Not_Exist()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        var workItem = MockDataProvider.GetTasks(1)[0];

        // Act
        var updatedProjectActivity = projectActivity.RemoveWorkItem(workItem);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }

    // # 3: A work item should not be removed from the project activity if it is null.
    [Test]
    public void WorkItem_Should_Not_Be_Removed_From_Activity_If_It_Is_Null()
    {
        // Arrange
        var projectActivity = MockDataProvider.GetProjectActivities(1)[0];
        WorkItem? workItem = null;

        // Act
        var updatedProjectActivity = projectActivity.RemoveWorkItem(workItem!);

        // Assert
        Assert.That(updatedProjectActivity.IsFailure, Is.True);
    }
}