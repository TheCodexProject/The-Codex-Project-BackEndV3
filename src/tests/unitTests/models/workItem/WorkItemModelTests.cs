using domain.models.workItem;
using domain.models.workItem.values;
using domain.shared;
using unitTests.utils;

namespace unitTests.models.workItem;

[TestFixture]
public class WorkItemModelTests
{
    // SECTION #1: Creation of Work Item.

    // # 1: A work item can be created with valid data.
    [Test]
    public void WorkItem_Should_Be_Created_With_Valid_Title_And_Project()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "New Work Item";

        // Act
        var result = WorkItem.Create(project, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Title, Is.EqualTo(title));
        });
    }

    // # 2: A work item cannot be created with an invalid title. (Too short)
    [Test]
    public void WorkItem_Should_Not_Be_Created_With_Invalid_Title_Too_Short()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "Ne";

        // Act
        var result = WorkItem.Create(project, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: A work item cannot be created with an invalid title. (Too long)
    [Test]
    public void WorkItem_Should_Not_Be_Created_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var title = "A".PadLeft(76);

        // Act
        var result = WorkItem.Create(project, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 4: A work item cannot be created with an invalid title. (Empty)
    [Test]
    public void WorkItem_Should_Not_Be_Created_With_Invalid_Title_Empty()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "";

        // Act
        var result = WorkItem.Create(project, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(1));
        });
    }

    // SECTION #2: Updates to Work Item.

    // SECTION #2.1: Update Title.

    // # 1: A work item's title can be updated with a valid title.
    [Test]
    public void WorkItem_Title_Should_Be_Updated_With_Valid_Title()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        const string newTitle = "Updated Title";

        // Act
        var result = workItem.UpdateTitle(newTitle);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Title, Is.EqualTo(newTitle));
        });
    }

    // # 2: A work item's title cannot be updated with an invalid title. (Too short)
    [Test]
    public void WorkItem_Title_Should_Not_Be_Updated_With_Invalid_Title_Too_Short()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        const string newTitle = "Ne";

        // Act
        var result = workItem.UpdateTitle(newTitle);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Title, Is.Not.EqualTo(newTitle));
        });
    }

    // # 3: A work item's title cannot be updated with an invalid title. (Too long)
    [Test]
    public void WorkItem_Title_Should_Not_Be_Updated_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var newTitle = "A".PadLeft(76);

        // Act
        var result = workItem.UpdateTitle(newTitle);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Title, Is.Not.EqualTo(newTitle));
        });
    }

    // # 4: A work item's title cannot be updated with an invalid title. (Empty)
    [Test]
    public void WorkItem_Title_Should_Not_Be_Updated_With_Invalid_Title_Empty()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        const string newTitle = "";

        // Act
        var result = workItem.UpdateTitle(newTitle);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Title, Is.Not.EqualTo(newTitle));
        });
    }

    // SECTION #2.2: Update Description.

    // # 1: A work item's description can be updated with a valid description.
    [Test]
    public void WorkItem_Description_Should_Be_Updated_With_Valid_Description()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        const string newDescription = "Updated Description";

        // Act
        var result = workItem.UpdateDescription(newDescription);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Description, Is.EqualTo(newDescription));
        });
    }

    // # 2: A work item's description cannot be updated with an invalid description. (More than 500 characters)
    [Test]
    public void WorkItem_Description_Should_Not_Be_Updated_With_Invalid_Description_Too_Long()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var newDescription = "A".PadLeft(501);

        // Act
        var result = workItem.UpdateDescription(newDescription);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Description, Is.Not.EqualTo(newDescription));
        });
    }

    // SECTION #2.3: Update Status.

    // # 1: A work item's status can be updated with a valid status.
    [TestCase(Status.Open)]
    [TestCase(Status.InProgress)]
    [TestCase(Status.ReadyForReview)]
    [TestCase(Status.Done)]
    [TestCase(Status.Closed)]
    public void WorkItem_Status_Should_Be_Updated_With_Valid_Status(Status value)
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = workItem.UpdateStatus(value);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Status, Is.EqualTo(value));
        });
    }

    // # 2: A work item's status cannot be updated with an invalid status. (None)
    [Test]
    public void WorkItem_Status_Should_Not_Be_Updated_With_Invalid_Status()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        const Status value = Status.None;

        // Act
        var result = workItem.UpdateStatus(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.4: Update Priority.

    // # 1: A work item's priority can be updated with a valid priority.
    [TestCase(Priority.Low)]
    [TestCase(Priority.Medium)]
    [TestCase(Priority.High)]
    [TestCase(Priority.Critical)]
    public void WorkItem_Priority_Should_Be_Updated_With_Valid_Priority(Priority value)
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = workItem.UpdatePriority(value);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Priority, Is.EqualTo(value));
        });
    }

    // # 2: A work item's priority cannot be updated with an invalid priority. (None)
    [Test]
    public void WorkItem_Priority_Should_Not_Be_Updated_With_Invalid_Priority()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        const Priority value = Priority.None;

        // Act
        var result = workItem.UpdatePriority(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.5: Update Type.

    // # 1: A work item's type can be updated with a valid type.
    [TestCase(ItemType.Task)]
    [TestCase(ItemType.Bug)]
    [TestCase(ItemType.UserStory)]
    [TestCase(ItemType.Epic)]
    public void WorkItem_Type_Should_Be_Updated_With_Valid_Type(ItemType value)
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = workItem.UpdateType(value);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Type, Is.EqualTo(value));
        });
    }

    // # 2: A work item's type cannot be updated with an invalid type. (None)
    [Test]
    public void WorkItem_Type_Should_Not_Be_Updated_With_Invalid_Type()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        const ItemType value = ItemType.None;

        // Act
        var result = workItem.UpdateType(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.6: Update Assigned To.

    // # 1: A work item's assigned to can be updated with a valid user.
    [Test]
    public void WorkItem_AssignedTo_Should_Be_Updated_With_Valid_User()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var user = MockDataProvider.GetUser();

        // Act
        var result = workItem.UpdateAssignedTo(user);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.AssignedTo, Is.EqualTo(user));
        });
    }

    // # 2: A work item's assigned to can be updated with a null user.
    [Test]
    public void WorkItem_AssignedTo_Should_Not_Be_Updated_With_Null_User()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = workItem.UpdateAssignedTo(null);

        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.7: Add Subitem.

    // # 1: A work item can have a subitem added to it.
    [Test]
    public void WorkItem_Should_Have_Subitem_Added()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var subitem = MockDataProvider.GetTask();

        // Act
        var result = workItem.AddSubitem(subitem);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Subitems, Has.Member(subitem));
            Assert.That(subitem.Parent, Is.EqualTo(workItem));
        });
    }

    // # 2: A work item cannot have a subitem added to it if it already has the subitem.
    [Test]
    public void WorkItem_Should_Not_Have_Subitem_Added_If_Already_Exists()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var subitem = MockDataProvider.GetTask();
        workItem.AddSubitem(subitem);

        // Act
        var result = workItem.AddSubitem(subitem);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Subitems, Has.Count.EqualTo(1));
        });
    }

    // # 3: A work item cannot have a subitem added to it if the subitem is null.
    [Test]
    public void WorkItem_Should_Not_Have_Subitem_Added_If_Null()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = workItem.AddSubitem(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Subitems, Is.Empty);
        });
    }

    // SECTION #2.8: Remove Subitem.

    // # 1: A work item can have a subitem removed from it.
    [Test]
    public void WorkItem_Should_Have_Subitem_Removed()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var subitem = MockDataProvider.GetTask();
        workItem.AddSubitem(subitem);

        // Act
        var result = workItem.RemoveSubitem(subitem);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Subitems, Has.No.Member(subitem));
            Assert.That(subitem.Parent, Is.Null);
        });
    }

    // # 2: A work item cannot have a subitem removed from it if it does not have the subitem.
    [Test]
    public void WorkItem_Should_Not_Have_Subitem_Removed_If_Does_Not_Exist()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var subitem = MockDataProvider.GetTask();

        // Act
        var result = workItem.RemoveSubitem(subitem);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Subitems, Is.Empty);
            Assert.That(subitem.Parent, Is.Null);
        });
    }

    // # 3: A work item cannot have a subitem removed from it if the subitem is null.
    [Test]
    public void WorkItem_Should_Not_Have_Subitem_Removed_If_Null()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = workItem.RemoveSubitem(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Subitems, Is.Empty);
        });
    }

    // SECTION #2.9: Add Activity.

    // # 1: A work item can have an activity added to it.
    [Test]
    public void WorkItem_Should_Have_Activity_Added()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var activity = MockDataProvider.GetProjectActivities(1)[0];

    // Act
        var result = workItem.AddActivity(activity);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Milestones, Has.Member(activity));
        });
    }

    // # 2: A work item cannot have an activity added to it if it already has the activity.
    [Test]
    public void WorkItem_Should_Not_Have_Activity_Added_If_Already_Exists()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var activity = MockDataProvider.GetProjectActivities(1)[0];
        workItem.AddActivity(activity);

        // Act
        var result = workItem.AddActivity(activity);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Milestones, Has.Count.EqualTo(1));
        });
    }

    // # 3: A work item cannot have an activity added to it if the activity is null.
    [Test]
    public void WorkItem_Should_Not_Have_Activity_Added_If_Null()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = workItem.AddActivity(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Milestones, Is.Empty);
        });
    }

    // SECTION #2.10: Remove Activity.

    // # 1: A work item can have an activity removed from it.
    [Test]
    public void WorkItem_Should_Have_Activity_Removed()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var activity = MockDataProvider.GetProjectActivities(1)[0];
        workItem.AddActivity(activity);

        // Act
        var result = workItem.RemoveActivity(activity);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(workItem.Milestones, Has.No.Member(activity));
            Assert.That(workItem.Milestones, Is.Empty);
        });
    }

    // # 2: A work item cannot have an activity removed from it if it does not have the activity.
    [Test]
    public void WorkItem_Should_Not_Have_Activity_Removed_If_Does_Not_Exist()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();
        var activity = MockDataProvider.GetProjectActivities(1)[0];

        // Act
        var result = workItem.RemoveActivity(activity);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Milestones, Is.Empty);
        });
    }

    // # 3: A work item cannot have an activity removed from it if the activity is null.
    [Test]
    public void WorkItem_Should_Not_Have_Activity_Removed_If_Null()
    {
        // Arrange
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = workItem.RemoveActivity(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(workItem.Milestones, Is.Empty);
        });
    }
}