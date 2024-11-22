using domain.models.project;
using domain.models.resource.values;
using domain.shared;
using unitTests.utils;

namespace unitTests.models.project;

public class ProjectModelTests
{
    // SECTION #1: Creation of Project

    // # 1: A project should be created with a valid title and workspace.
    [Test]
    public void Project_Should_Be_Created_With_Valid_Title_And_Workspace()
    {
        // Arrange
        var workspace = MockDataProvider.GetWorkspace();
        const string title = "Test Project";

        // Act
        var project = Project.Create(workspace, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(project.IsSuccess, Is.True);
            Assert.That(project.Value.Title, Is.EqualTo(title));
            Assert.That(project.Value.Workspace, Is.EqualTo(workspace));
        });
    }

    // # 2: A project should not be created with an invalid title. (Too short)
    [Test]
    public void Project_Should_Not_Be_Created_With_Invalid_Title_Too_Short()
    {
        // Arrange
        var workspace = MockDataProvider.GetWorkspace();
        const string title = "T";

        // Act
        var project = Project.Create(workspace, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(project.IsFailure, Is.True);
            Assert.That(project.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 3: A project should not be created with an invalid title. (Too long)
    [Test]
    public void Project_Should_Not_Be_Created_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var workspace = MockDataProvider.GetWorkspace();
        var title = "a".PadLeft(101);

        // Act
        var project = Project.Create(workspace, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(project.IsFailure, Is.True);
            Assert.That(project.Errors.Count, Is.EqualTo(1));
        });
    }

    // # 4: A project should not be created with an invalid title. (Empty)
    [Test]
    public void Project_Should_Not_Be_Created_With_Invalid_Title_Empty()
    {
        // Arrange
        var workspace = MockDataProvider.GetWorkspace();
        const string title = "";

        // Act
        var project = Project.Create(workspace, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(project.IsFailure, Is.True);
            Assert.That(project.Errors.Count, Is.EqualTo(1));
        });
    }

    // SECTION #2: Updates to Project

    // SECTION #2.1: Update Title

    // # 1: A project's title should be updated with a valid title.
    [Test]
    public void Project_Title_Should_Be_Updated_With_Valid_Title()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "Updated Title";

        // Act
        var result = project.UpdateTitle(title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Title, Is.EqualTo(title));
        });
    }

    // # 2: A project's title should not be updated with an invalid title. (Too short)
    [Test]
    public void Project_Title_Should_Not_Be_Updated_With_Invalid_Title_Too_Short()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "T";

        // Act
        var result = project.UpdateTitle(title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Title, Is.Not.EqualTo(title));
        });
    }

    // # 3: A project's title should not be updated with an invalid title. (Too long)
    [Test]
    public void Project_Title_Should_Not_Be_Updated_With_Invalid_Title_Too_Long()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var title = "a".PadLeft(101);

        // Act
        var result = project.UpdateTitle(title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Title, Is.Not.EqualTo(title));
        });
    }

    // # 4: A project's title should not be updated with an invalid title. (Empty)
    [Test]
    public void Project_Title_Should_Not_Be_Updated_With_Invalid_Title_Empty()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "";

        // Act
        var result = project.UpdateTitle(title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Title, Is.Not.EqualTo(title));
        });
    }

    // SECTION #2.2: Update Description

    // # 1: A project's description should be updated with a valid description.
    [Test]
    public void Project_Description_Should_Be_Updated_With_Valid_Description()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string description = "Updated Description";

        // Act
        var result = project.UpdateDescription(description);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Description, Is.EqualTo(description));
        });
    }

    // # 2: A project's description should not be updated with an invalid description. (more than 500 characters)
    [Test]
    public void Project_Description_Should_Not_Be_Updated_With_Invalid_Description_Too_Long()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var description = "a".PadLeft(501);

        // Act
        var result = project.UpdateDescription(description);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Description, Is.Not.EqualTo(description));
        });
    }

    // SECTION #2.3: Update Status

    // # 1: A project's status should be updated with a valid status.
    [Test]
    public void Project_Status_Should_Be_Updated_With_Valid_Status()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var status = Status.InProgress;

        // Act
        var result = project.UpdateStatus(status);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Status, Is.EqualTo(status));
        });
    }

    // # 2: A project's status should not be updated with an invalid status. (None)
    [Test]
    public void Project_Status_Should_Not_Be_Updated_With_Invalid_Status_None()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var status = Status.None;

        // Act
        var result = project.UpdateStatus(status);

        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.4: Update Priority

    // # 1: A project's priority should be updated with a valid priority.
    [Test]
    public void Project_Priority_Should_Be_Updated_With_Valid_Priority()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var priority = Priority.High;

        // Act
        var result = project.UpdatePriority(priority);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Priority, Is.EqualTo(priority));
        });
    }

    // # 2: A project's priority should not be updated with an invalid priority. (None)
    [Test]
    public void Project_Priority_Should_Not_Be_Updated_With_Invalid_Priority_None()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var priority = Priority.None;

        // Act
        var result = project.UpdatePriority(priority);

        Assert.That(result.IsFailure, Is.True);
    }

    // SECTION #2.5: Update Time Frame

    // # 1: A project's time frame should be updated with a valid time frame.
    [Test]
    public void Project_TimeFrame_Should_Be_Updated_With_Valid_TimeFrame()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(7);

        // Act
        var result = project.UpdateTimeRange(startDate, endDate);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Start, Is.EqualTo(startDate));
            Assert.That(project.End, Is.EqualTo(endDate));
        });
    }

    // # 2: A project's time frame should not be updated with an invalid time frame. (Start date is not set)
    [Test]
    public void Project_TimeFrame_Should_Not_Be_Updated_With_Invalid_TimeFrame_StartDate_Not_Set()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var startDate = DateTime.MinValue;
        var endDate = DateTime.Today.AddDays(7);

        // Act
        var result = project.UpdateTimeRange(startDate, endDate);

        Assert.That(result.IsFailure, Is.True);
    }

    // # 3: A project's time frame should not be updated with an invalid time frame. (End date is not set)
    [Test]
    public void Project_TimeFrame_Should_Not_Be_Updated_With_Invalid_TimeFrame_EndDate_Not_Set()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var startDate = DateTime.Today;
        var endDate = DateTime.MinValue;

        // Act
        var result = project.UpdateTimeRange(startDate, endDate);

        Assert.That(result.IsFailure, Is.True);
    }

    // # 4: A project's time frame should not be updated with an invalid time frame. (Start date is in the past)
    [Test]
    public void Project_TimeFrame_Should_Not_Be_Updated_With_Invalid_TimeFrame_StartDate_In_The_Past()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var startDate = DateTime.Today.AddDays(-1);
        var endDate = DateTime.Today;

        // Act
        var result = project.UpdateTimeRange(startDate, endDate);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Start, Is.Not.EqualTo(startDate));
            Assert.That(project.End, Is.Not.EqualTo(endDate));
        });
    }

    // # 5: A project's time frame should not be updated with an invalid time frame. (Start date is after end date)
    [Test]
    public void Project_TimeFrame_Should_Not_Be_Updated_With_Invalid_TimeFrame_StartDate_After_EndDate()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var startDate = DateTime.Today.AddDays(7);
        var endDate = DateTime.Today;

        // Act
        var result = project.UpdateTimeRange(startDate, endDate);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Start, Is.Not.EqualTo(startDate));
            Assert.That(project.End, Is.Not.EqualTo(endDate));
        });
    }

    // # 6: A project's time frame should not be updated with an invalid time frame. (End date is before start date)
    [Test]
    public void Project_TimeFrame_Should_Not_Be_Updated_With_Invalid_TimeFrame_EndDate_Before_StartDate()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(-7);

        // Act
        var result = project.UpdateTimeRange(startDate, endDate);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Start, Is.Not.EqualTo(startDate));
            Assert.That(project.End, Is.Not.EqualTo(endDate));
        });
    }

    // # 7: A project's time frame should not be updated with an invalid time frame. (Start date is the same as end date)
    [Test]
    public void Project_TimeFrame_Should_Not_Be_Updated_With_Invalid_TimeFrame_StartDate_Same_As_EndDate()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var startDate = DateTime.Today;
        var endDate = DateTime.Today;

        // Act
        var result = project.UpdateTimeRange(startDate, endDate);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Start, Is.Not.EqualTo(startDate));
            Assert.That(project.End, Is.Not.EqualTo(endDate));
        });
    }

    // SECTION #2.6: Add Task

    // # 1: A task should be added to the project.
    [Test]
    public void Task_Should_Be_Added_To_Project()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var task = MockDataProvider.GetTask();

        // Act
        var result = project.AddTask(task);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Tasks, Has.Member(task));
        });
    }

    // # 2: A task should not be added to the project if it already exists.
    [Test]
    public void Task_Should_Not_Be_Added_To_Project_If_It_Already_Exists()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var task = MockDataProvider.GetTask();
        project.AddTask(task);

        // Act
        var result = project.AddTask(task);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Tasks, Has.Member(task));
        });
    }

    // # 3: A task should not be added to the project if it is null.
    [Test]
    public void Task_Should_Not_Be_Added_To_Project_If_It_Is_Null()
    {
        // Arrange
        var project = MockDataProvider.GetProject();

        // Act
        var result = project.AddTask(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Tasks, Is.Empty);
        });
    }

    // SECTION #2.7: Remove Task

    // # 1: A task should be removed from the project.
    [Test]
    public void Task_Should_Be_Removed_From_Project()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var task = MockDataProvider.GetTask();
        project.AddTask(task);

        // Act
        var result = project.RemoveTask(task);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Tasks, Has.No.Member(task));
        });
    }

    // # 2: A task should not be removed from the project if it does not exist.
    [Test]
    public void Task_Should_Not_Be_Removed_From_Project_If_It_Does_Not_Exist()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var task = MockDataProvider.GetTask();

        // Act
        var result = project.RemoveTask(task);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Tasks, Has.No.Member(task));
        });
    }

    // # 3: A task should not be removed from the project if it is null.
    [Test]
    public void Task_Should_Not_Be_Removed_From_Project_If_It_Is_Null()
    {
        // Arrange
        var project = MockDataProvider.GetProject();

        // Act
        var result = project.RemoveTask(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Tasks, Is.Empty);
        });
    }

    // SECTION #2.8: Add Resource

    // # 1: A resource should be added to the project.
    [Test]
    public void Resource_Should_Be_Added_To_Project()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var resource = MockDataProvider.GetResources(1,ResourceLevel.Project)[0];

        // Act
        var result = project.AddResource(resource);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Resources, Has.Member(resource));
        });
    }

    // # 2: A resource should not be added to the project if it already exists.
    [Test]
    public void Resource_Should_Not_Be_Added_To_Project_If_It_Already_Exists()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var resource = MockDataProvider.GetResources(1,ResourceLevel.Project)[0];

        project.AddResource(resource);

        // Act
        var result = project.AddResource(resource);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Resources, Has.Member(resource));
        });
    }

    // # 3: A resource should not be added to the project if it is null.
    [Test]
    public void Resource_Should_Not_Be_Added_To_Project_If_It_Is_Null()
    {
        // Arrange
        var project = MockDataProvider.GetProject();

        // Act
        var result = project.AddResource(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Resources, Is.Empty);
        });
    }

    // SECTION #2.9: Remove Resource

    // # 1: A resource should be removed from the project.
    [Test]
    public void Resource_Should_Be_Removed_From_Project()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var resource = MockDataProvider.GetResources(1,ResourceLevel.Project)[0];
        project.AddResource(resource);

        // Act
        var result = project.RemoveResource(resource);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.Resources, Has.No.Member(resource));
        });
    }

    // # 2: A resource should not be removed from the project if it does not exist.
    [Test]
    public void Resource_Should_Not_Be_Removed_From_Project_If_It_Does_Not_Exist()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var resource = MockDataProvider.GetResources(1,ResourceLevel.Project)[0];

        // Act
        var result = project.RemoveResource(resource);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Resources, Has.No.Member(resource));
        });
    }

    // # 3: A resource should not be removed from the project if it is null.
    [Test]
    public void Resource_Should_Not_Be_Removed_From_Project_If_It_Is_Null()
    {
        // Arrange
        var project = MockDataProvider.GetProject();

        // Act
        var result = project.RemoveResource(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.Resources, Is.Empty);
        });
    }

    // SECTION #2.10: Add Activity

    // # 1: An activity should be added to the project.
    [Test]
    public void Activity_Should_Be_Added_To_Project()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var activity = MockDataProvider.GetProjectActivities(1)[0];

        // Act
        var result = project.AddActivity(activity);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.ProjectActivities, Has.Member(activity));
        });
    }

    // # 2: An activity should not be added to the project if it already exists.
    [Test]
    public void Activity_Should_Not_Be_Added_To_Project_If_It_Already_Exists()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var activity = MockDataProvider.GetProjectActivities(1)[0];
        project.AddActivity(activity);

        // Act
        var result = project.AddActivity(activity);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.ProjectActivities, Has.Member(activity));
        });
    }

    // # 3: An activity should not be added to the project if it is null.
    [Test]
    public void Activity_Should_Not_Be_Added_To_Project_If_It_Is_Null()
    {
        // Arrange
        var project = MockDataProvider.GetProject();

        // Act
        var result = project.AddActivity(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.ProjectActivities, Is.Empty);
        });
    }

    // SECTION #2.11: Remove Activity

    // # 1: An activity should be removed from the project.
    [Test]
    public void Activity_Should_Be_Removed_From_Project()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var activity = MockDataProvider.GetProjectActivities(1)[0];
        project.AddActivity(activity);

        // Act
        var result = project.RemoveActivity(activity);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(project.ProjectActivities, Has.No.Member(activity));
        });
    }

    // # 2: An activity should not be removed from the project if it does not exist.
    [Test]
    public void Activity_Should_Not_Be_Removed_From_Project_If_It_Does_Not_Exist()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        var activity = MockDataProvider.GetProjectActivities(1)[0];

        // Act
        var result = project.RemoveActivity(activity);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.ProjectActivities, Has.No.Member(activity));
        });
    }

    // # 3: An activity should not be removed from the project if it is null.
    [Test]
    public void Activity_Should_Not_Be_Removed_From_Project_If_It_Is_Null()
    {
        // Arrange
        var project = MockDataProvider.GetProject();

        // Act
        var result = project.RemoveActivity(null!);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(project.ProjectActivities, Is.Empty);
        });
    }
}