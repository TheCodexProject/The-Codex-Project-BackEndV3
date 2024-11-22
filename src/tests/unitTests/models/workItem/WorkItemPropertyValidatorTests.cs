using domain.models.projectActivity;
using domain.models.user;
using domain.models.workItem;
using domain.models.workItem.values;
using domain.shared;
using unitTests.utils;

namespace unitTests.models.workItem;

[TestFixture]
public class WorkItemPropertyValidatorTests
{
    // SECTION #1: Validate Title.

    // # 1: Empty, whitespace or null should be invalid.
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Empty_Title_Should_Be_Invalid(string? value)
    {
        // Act
        var result = WorkItemPropertyValidator.ValidateTitle(value!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }


    // # 2: Title with less than 3 characters should be invalid.
    [Theory]
    [TestCase("a")]
    [TestCase("ab")]
    public void Title_Cannot_Be_Less_Than_3_Characters(string value)
    {
        // Act
        var result = WorkItemPropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Title with more than 75 characters should be invalid.
    [Test]
    public void Title_Cannot_Be_More_Than_75_Characters()
    {
        // Arrange
        var title = new string('a', 76);

        // Act
        var result = WorkItemPropertyValidator.ValidateTitle(title);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: Title with 3 to 75 characters should be valid.
    [Theory]
    [TestCase("Alpha")]
    [TestCase("Alpha Beta")]
    [TestCase("MIN")]
    public void Title_Can_Be_Between_3_And_75_Characters(string value)
    {
        // Act
        var result = WorkItemPropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #2: Validate Description.

    // # 1: Description with more than 500 characters should be invalid.
    [Test]
    public void Description_Cannot_Be_More_Than_500_Characters()
    {
        // Arrange
        var description = new string('a', 501);

        // Act
        var result = WorkItemPropertyValidator.ValidateDescription(description);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Description with 500 characters should be valid.
    [Test]
    public void Description_Can_Be_500_Characters()
    {
        // Arrange
        var description = new string('a', 500);

        // Act
        var result = WorkItemPropertyValidator.ValidateDescription(description);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #3: Validate Status.

    // # 1: Status None should be invalid.
    [Test]
    public void Status_None_Should_Be_Invalid()
    {
        // Arrange
        const Status status = Status.None;

        // Act
        var result = WorkItemPropertyValidator.ValidateStatus(status);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Status other than None should be valid.
    [TestCase(Status.Open)]
    [TestCase(Status.InProgress)]
    [TestCase(Status.ReadyForReview)]
    [TestCase(Status.Done)]
    [TestCase(Status.Closed)]
    public void Status_Other_Than_None_Should_Be_Valid(Status value)
    {
        // Act
        var result = WorkItemPropertyValidator.ValidateStatus(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #4: Validate Priority.

    // # 1: Priority None should be invalid.
    [Test]
    public void Priority_None_Should_Be_Invalid()
    {
        // Arrange
        const Priority priority = Priority.None;

        // Act
        var result = WorkItemPropertyValidator.ValidatePriority(priority);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Priority other than None should be valid.
    [TestCase(Priority.Low)]
    [TestCase(Priority.Medium)]
    [TestCase(Priority.High)]
    [TestCase(Priority.Critical)]
    public void Priority_Other_Than_None_Should_Be_Valid(Priority value)
    {
        // Act
        var result = WorkItemPropertyValidator.ValidatePriority(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #5: Validate Type.

    // # 1: Type None should be invalid.
    [Test]
    public void Type_None_Should_Be_Invalid()
    {
        // Arrange
        const ItemType type = ItemType.None;

        // Act
        var result = WorkItemPropertyValidator.ValidateType(type);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Type other than None should be valid.
    [TestCase(ItemType.Bug)]
    [TestCase(ItemType.UserStory)]
    [TestCase(ItemType.Task)]
    [TestCase(ItemType.Epic)]
    public void Type_Other_Than_None_Should_Be_Valid(ItemType value)
    {
        // Act
        var result = WorkItemPropertyValidator.ValidateType(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #6: Validate Assigned To.

    // # 1: Assigned To null should be invalid.
    [Test]
    public void Assigned_To_Null_Should_Be_Invalid()
    {
        // Arrange
        User? assignedTo = null;

        // Act
        var result = WorkItemPropertyValidator.ValidateAssignedTo(assignedTo);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Assigned To other than null should be valid.
    [Test]
    public void Assigned_To_Other_Than_Null_Should_Be_Valid()
    {
        // Arrange
        var assignedTo = MockDataProvider.GetUser();

        // Act
        var result = WorkItemPropertyValidator.ValidateAssignedTo(assignedTo);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #7: Validate Add Subitem.

    // # 1: Subitem null should be invalid.
    [Test]
    public void Subitem_Null_Should_Be_Invalid()
    {
        // Arrange
        var subitems = new List<WorkItem>();
        WorkItem? subitem = null;

        // Act
        var result = WorkItemPropertyValidator.ValidateAddSubitem(subitem, subitems);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Subitem already exists in the list should be invalid.
    [Test]
    public void Subitem_Already_Exists_In_The_List_Should_Be_Invalid()
    {
        // Arrange
        var subitems = MockDataProvider.GetTasks(3);
        var subitem = subitems[0];

        // Act
        var result = WorkItemPropertyValidator.ValidateAddSubitem(subitem, subitems);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Subitem does not exist in the list should be valid.
    [Test]
    public void Subitem_Does_Not_Exist_In_The_List_Should_Be_Valid()
    {
        // Arrange
        var subitems = MockDataProvider.GetTasks(3);
        var subitem = MockDataProvider.GetTask();

        // Act
        var result = WorkItemPropertyValidator.ValidateAddSubitem(subitem, subitems);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #8: Validate Remove Subitem.

    // # 1: Subitem null should be invalid.
    [Test]
    public void Remove_Subitem_Null_Should_Be_Invalid()
    {
        // Arrange
        var subitems = new List<WorkItem>();
        WorkItem? subitem = null;

        // Act
        var result = WorkItemPropertyValidator.ValidateRemoveSubitem(subitem, subitems);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Subitem does not exist in the list should be invalid.
    [Test]
    public void Remove_Subitem_Does_Not_Exist_In_The_List_Should_Be_Invalid()
    {
        // Arrange
        var subitems = MockDataProvider.GetTasks(3);
        var subitem = MockDataProvider.GetTask();

        // Act
        var result = WorkItemPropertyValidator.ValidateRemoveSubitem(subitem, subitems);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Subitem exists in the list should be valid.
    [Test]
    public void Remove_Subitem_Exists_In_The_List_Should_Be_Valid()
    {
        // Arrange
        var subitems = MockDataProvider.GetTasks(3);
        var subitem = subitems[0];

        // Act
        var result = WorkItemPropertyValidator.ValidateRemoveSubitem(subitem, subitems);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #9: Validate Add Activity.

    // # 1: Activity null should be invalid.
    [Test]
    public void Add_Activity_Null_Should_Be_Invalid()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        ProjectActivity? activity = null;

        // Act
        var result = WorkItemPropertyValidator.ValidateAddActivity(activity, activities);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Activity already exists in the list should be invalid.
    [Test]
    public void Add_Activity_Already_Exists_In_The_List_Should_Be_Invalid()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        var activity = activities[0];

        // Act
        var result = WorkItemPropertyValidator.ValidateAddActivity(activity, activities);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Activity does not exist in the list should be valid.
    [Test]
    public void Add_Activity_Does_Not_Exist_In_The_List_Should_Be_Valid()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        var activity = MockDataProvider.GetProjectActivities(1,true)[0];

        // Act
        var result = WorkItemPropertyValidator.ValidateAddActivity(activity, activities);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #10: Validate Remove Activity.

    // # 1: Activity null should be invalid.
    [Test]
    public void Remove_Activity_Null_Should_Be_Invalid()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        ProjectActivity? activity = null;

        // Act
        var result = WorkItemPropertyValidator.ValidateRemoveActivity(activity, activities);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Activity does not exist in the list should be invalid.
    [Test]
    public void Remove_Activity_Does_Not_Exist_In_The_List_Should_Be_Invalid()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        var activity = MockDataProvider.GetProjectActivities(1,true)[0];

        // Act
        var result = WorkItemPropertyValidator.ValidateRemoveActivity(activity, activities);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Activity exists in the list should be valid.
    [Test]
    public void Remove_Activity_Exists_In_The_List_Should_Be_Valid()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        var activity = activities[0];

        // Act
        var result = WorkItemPropertyValidator.ValidateRemoveActivity(activity, activities);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }
}