using domain.models.organization;
using domain.models.project;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using domain.models.resource;
using domain.models.resource.values;
using domain.models.user;
using domain.models.workItem;
using domain.models.workspace;
using domain.shared;
using unitTests.utils;

namespace unitTests.models.project;

public class ProjectPropertyValidatorTests
{
    // SECTION #1: Validate Title

    // # 1: Title is empty, whitespace, or null.
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Empty_Title_Should_Be_Invalid(string? title)
    {
        // Act
        var result = ProjectPropertyValidator.ValidateTitle(title!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Title is less than 3 characters.
    [Theory]
    [TestCase("A")]
    [TestCase("AB")]
    public void Title_Should_Be_At_Least_3_Characters(string title)
    {
        // Act
        var result = ProjectPropertyValidator.ValidateTitle(title);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Title is more than 75 characters.
    [Test]
    public void Title_Should_Be_At_Most_75_Characters()
    {
        // Arrange
        var title = new string('A', 76);

        // Act
        var result = ProjectPropertyValidator.ValidateTitle(title);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: Title should be valid
    [Theory]
    [TestCase("Alpha Bravo Charlie")]
    [TestCase("Alpha")]
    [TestCase("MIN")]
    public void Title_Should_Be_Valid(string value)
    {
        // Act
        var result = ProjectPropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #2: Validate Description

    // # 1: Description is more than 500 characters.
    [Test]
    public void Description_Cannot_Be_More_Than_500_Characters()
    {
        // Arrange
        var description = new string('A', 501);

        // Act
        var result = ProjectPropertyValidator.ValidateDescription(description);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Description should be valid
    [Theory]
    [TestCase("Alpha Bravo Charlie")]
    [TestCase("Alpha")]
    [TestCase("MIN")]
    public void Description_Should_Be_Valid(string value)
    {
        // Act
        var result = ProjectPropertyValidator.ValidateDescription(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #3: Validate Status

    // # 1: Status is None.
    [Test]
    public void Status_Cannot_Be_Set_To_None()
    {
        // Arrange
        const Status status = Status.None;

        // Act
        var result = ProjectPropertyValidator.ValidateStatus(status);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Status should be valid
    [TestCase(Status.Open)]
    [TestCase(Status.InProgress)]
    [TestCase(Status.ReadyForReview)]
    [TestCase(Status.Done)]
    [TestCase(Status.Closed)]
    public void Status_Should_Be_Valid(Status value)
    {
        // Act
        var result = ProjectPropertyValidator.ValidateStatus(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #4: Validate Priority

    // # 1: Priority is None.
    [Test]
    public void Priority_Cannot_Be_Set_To_None()
    {
        // Arrange
        const Priority priority = Priority.None;

        // Act
        var result = ProjectPropertyValidator.ValidatePriority(priority);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Priority should be valid
    [TestCase(Priority.Low)]
    [TestCase(Priority.Medium)]
    [TestCase(Priority.High)]
    [TestCase(Priority.Critical)]
    public void Priority_Should_Be_Valid(Priority value)
    {
        // Arrange

        // Act
        var result = ProjectPropertyValidator.ValidatePriority(value);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Errors.Count, Is.EqualTo(0));
        });
    }

    // SECTION #5: Validate Time Frame

    // # 1: Start date is not set.
    [Test]
    public void StartDate_Is_Not_Set()
    {
        // Arrange
        var start = DateTime.MinValue;
        var end = DateTime.Today;

        // Act
        var result = ProjectPropertyValidator.ValidateTimeRange(start, end);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: End date is not set.
    [Test]
    public void EndDate_Is_Not_Set()
    {
        // Arrange
        var start = DateTime.Today;
        var end = DateTime.MinValue;

        // Act
        var result = ProjectPropertyValidator.ValidateTimeRange(start, end);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Start date is in the past.
    [Test]
    public void StartDate_Is_In_The_Past()
    {
        // Arrange
        var start = DateTime.Today.AddDays(-1);
        var end = DateTime.Today;

        // Act
        var result = ProjectPropertyValidator.ValidateTimeRange(start, end);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: Start date is after the end date.
    [Test]
    public void StartDate_Is_After_The_EndDate()
    {
        // Arrange
        var start = DateTime.Today.AddDays(1);
        var end = DateTime.Today;

        // Act
        var result = ProjectPropertyValidator.ValidateTimeRange(start, end);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 5: End date is before the start date.
    [Test]
    public void EndDate_Is_Before_StartDate()
    {
        // Arrange
        var start = DateTime.Today;
        var end = DateTime.Today.AddDays(-1);

        // Act
        var result = ProjectPropertyValidator.ValidateTimeRange(start, end);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 6: Start date is the same as the end date.
    [Test]
    public void StartDate_Is_The_Same_As_EndDate()
    {
        // Arrange
        var start = DateTime.Today;
        var end = DateTime.Today;

        // Act
        var result = ProjectPropertyValidator.ValidateTimeRange(start, end);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 7: Time frame should be valid
    [Test]
    public void TimeRange_Should_Be_Valid()
    {
        // Arrange
        var start = DateTime.Today;
        var end = DateTime.Today.AddDays(1);

        // Act
        var result = ProjectPropertyValidator.ValidateTimeRange(start, end);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #6: Validate Add Task
    // # 1: Task is null.
    [Test]
    public void Task_Should_Not_Be_Null()
    {
        // Arrange
        WorkItem? task = null;
        var tasks = MockDataProvider.GetTasks(3);

        // Act
        var result = ProjectPropertyValidator.ValidateAddWorkItem(task, tasks);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Task is already in the list.
    [Test]
    public void Task_Should_Already_Exist_In_List()
    {
        // Arrange
        var tasks = MockDataProvider.GetTasks(3);
        var task = tasks[0];

        // Act
        var result = ProjectPropertyValidator.ValidateAddWorkItem(task, tasks);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Task should be valid
    [Test]
    public void Task_Should_Not_Already_Exist_In_List()
    {
        // Arrange
        var tasks = MockDataProvider.GetTasks(3);
        var task = WorkItem.Create(tasks[0].Project, "Task 4").Value;

        // Act
        var result = ProjectPropertyValidator.ValidateAddWorkItem(task, tasks);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #7: Validate Remove Task

    // # 1: Task is null.
    [Test]
    public void Task_Should_Not_Be_Null_For_Removal()
    {
        // Arrange
        WorkItem? task = null;
        var tasks = MockDataProvider.GetTasks(3);

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveWorkItem(task, tasks);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Task is not in the list.
    [Test]
    public void Task_Should_Not_Exist_In_List_For_Removal()
    {
        // Arrange
        var tasks = MockDataProvider.GetTasks(3);
        var task = WorkItem.Create(tasks[0].Project, "Task 4").Value;

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveWorkItem(task, tasks);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Task should be valid
    [Test]
    public void Task_Should_Exist_In_List_For_Removal()
    {
        // Arrange
        var tasks = MockDataProvider.GetTasks(3);
        var task = tasks[0];

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveWorkItem(task, tasks);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #8: Validate Add Resource

    // # 1: Resource is null.
    [Test]
    public void Resource_Should_Not_Be_Null()
    {
        // Arrange
        Resource? resource = null;
        var resources = MockDataProvider.GetResources(3,ResourceLevel.Project);

        // Act
        var result = ProjectPropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Resource is already in the list.
    [Test]
    public void Resource_Should_Already_Exist_In_List()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3,ResourceLevel.Project);
        var resource = resources[0];

        // Act
        var result = ProjectPropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Resource should be valid
    [Test]
    public void Resource_Should_Not_Already_Exist_In_List()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3,ResourceLevel.Project);
        var resource = Resource.Create( "Resource 4","URL...",resources[0].OwnerId ,ResourceLevel.Project).Value;

        // Act
        var result = ProjectPropertyValidator.ValidateAddResource(resource, resources);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #9: Validate Remove Resource

    // # 1: Resource is null.
    [Test]
    public void Resource_Should_Not_Be_Null_For_Removal()
    {
        // Arrange
        Resource? resource = null;
        var resources = MockDataProvider.GetResources(3,ResourceLevel.Project);

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Resource is not in the list.
    [Test]
    public void Resource_Should_Not_Exist_In_List_For_Removal()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3,ResourceLevel.Project);
        var resource = Resource.Create( "Resource 4","URL...",resources[0].OwnerId ,ResourceLevel.Project).Value;

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Resource should be valid
    [Test]
    public void Resource_Should_Exist_In_List_For_Removal()
    {
        // Arrange
        var resources = MockDataProvider.GetResources(3,ResourceLevel.Project);
        var resource = resources[0];

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveResource(resource, resources);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #10: Validate Add Activity

    // # 1: Activity is null.
    [Test]
    public void Activity_Should_Not_Be_Null()
    {
        // Arrange
        ProjectActivity? activity = null;
        var activities = MockDataProvider.GetProjectActivities(3);

        // Act
        var result = ProjectPropertyValidator.ValidateAddActivity(activity, activities);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Activity is already in the list.
    [Test]
    public void Activity_Should_Already_Exist_In_List()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        var activity = activities[0];

        // Act
        var result = ProjectPropertyValidator.ValidateAddActivity(activity, activities);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Activity should be valid
    [Test]
    public void Activity_Should_Not_Already_Exist_In_List()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        var activity = MockDataProvider.GetProjectActivities(1,true)[0];

        // Act
        var result = ProjectPropertyValidator.ValidateAddActivity(activity, activities);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #11: Validate Remove Activity

    // # 1: Activity is null.
    [Test]
    public void Activity_Should_Not_Be_Null_For_Removal()
    {
        // Arrange
        ProjectActivity? activity = null;
        var activities = MockDataProvider.GetProjectActivities(3);

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveActivity(activity, activities);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Activity is not in the list.
    [Test]
    public void Activity_Should_Not_Exist_In_List_For_Removal()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        var activity = MockDataProvider.GetProjectActivities(1,true)[0];

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveActivity(activity, activities);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Activity should be valid
    [Test]
    public void Activity_Should_Exist_In_List_For_Removal()
    {
        // Arrange
        var activities = MockDataProvider.GetProjectActivities(3);
        var activity = activities[0];

        // Act
        var result = ProjectPropertyValidator.ValidateRemoveActivity(activity, activities);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }
}