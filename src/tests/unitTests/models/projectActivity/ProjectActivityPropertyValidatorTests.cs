using domain.models.projectActivity;
using domain.models.projectActivity.value;
using domain.models.workItem;
using unitTests.utils;

namespace unitTests.models.projectActivity;

[TestFixture]
public class ProjectActivityPropertyValidatorTests
{
    // SECTION #1: Validate Title

    // # 1: Empty, whitespace or null should be invalid
    [Theory]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]

    public void Empty_Title_Should_Be_Invalid(string? title)
    {
        // Act
        var result = ProjectActivityPropertyValidator.ValidateTitle(title!);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Title with less than 3 characters should be invalid
    [Theory]
    [TestCase("a")]
    [TestCase("ab")]
    public void Title_Should_Be_At_Least_3_Characters(string title)
    {
        // Act
        var result = ProjectActivityPropertyValidator.ValidateTitle(title);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Title with more than 75 characters should be invalid
    [Test]
    public void Title_Should_Be_At_Most_75_Characters()
    {
        // Arrange
        var title = new string('a', 76);

        // Act
        var result = ProjectActivityPropertyValidator.ValidateTitle(title);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 4: Title with 3 to 75 characters should be valid
    [TestCase("Alpha")]
    [TestCase("Alpha Beta")]
    [TestCase("MIN")]
    public void Title_Should_Be_Valid(string value)
    {
        // Act
        var result = ProjectActivityPropertyValidator.ValidateTitle(value);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #2: Validate Description

    // # 1: Description with more than 500 characters should be invalid
    [Test]
    public void Description_Should_Be_At_Most_500_Characters()
    {
        // Arrange
        var description = new string('a', 501);

        // Act
        var result = ProjectActivityPropertyValidator.ValidateDescription(description);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Description with 500 characters should be valid
    [Test]
    public void Description_Should_Be_Valid()
    {
        // Arrange
        var description = new string('a', 500);

        // Act
        var result = ProjectActivityPropertyValidator.ValidateDescription(description);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #3: Validate Type

    // # 1: Type None should be invalid
    [Test]
    public void Type_None_Should_Be_Invalid()
    {
        // Arrange
        var type = ProjectActivityType.None;

        // Act
        var result = ProjectActivityPropertyValidator.ValidateType(type);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Any other type should be valid
    [TestCase(ProjectActivityType.Iteration)]
    [TestCase(ProjectActivityType.Milestone)]
    public void Type_Should_Be_Valid(ProjectActivityType type)
    {
        // Act
        var result = ProjectActivityPropertyValidator.ValidateType(type);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #4: Validate Add WorkItem

    // # 1: Work item is null, should be invalid
    [Test]
    public void Null_WorkItem_Should_Be_Invalid()
    {
        // Arrange
        var workItems = MockDataProvider.GetTasks(3);
        WorkItem? workItem = null;

        // Act
        var result = ProjectActivityPropertyValidator.ValidateAddWorkItem(workItem, workItems);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Work item is already in the list, should be invalid
    [Test]
    public void WorkItem_Already_In_List_Should_Be_Invalid()
    {
        // Arrange
        var workItems = MockDataProvider.GetTasks(3);
        var workItem = workItems[0];

        // Act
        var result = ProjectActivityPropertyValidator.ValidateAddWorkItem(workItem, workItems);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Work item is not in the list, should be valid
    [Test]
    public void WorkItem_Not_In_List_Should_Be_Valid()
    {
        // Arrange
        var workItems = MockDataProvider.GetTasks(3);
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = ProjectActivityPropertyValidator.ValidateAddWorkItem(workItem, workItems);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }

    // SECTION #5: Validate Remove WorkItem

    // # 1: Work item is null, should be invalid
    [Test]
    public void Null_WorkItem_Should_Be_Invalid_For_Removal()
    {
        // Arrange
        var workItems = MockDataProvider.GetTasks(3);
        WorkItem? workItem = null;

        // Act
        var result = ProjectActivityPropertyValidator.ValidateRemoveWorkItem(workItem, workItems);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 2: Work item is not in the list, should be invalid
    [Test]
    public void WorkItem_Not_In_List_Should_Be_Invalid_For_Removal()
    {
        // Arrange
        var workItems = MockDataProvider.GetTasks(3);
        var workItem = MockDataProvider.GetTask();

        // Act
        var result = ProjectActivityPropertyValidator.ValidateRemoveWorkItem(workItem, workItems);

        // Assert
        Assert.That(result.IsFailure, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(1));
    }

    // # 3: Work item is in the list, should be valid
    [Test]
    public void WorkItem_In_List_Should_Be_Valid_For_Removal()
    {
        // Arrange
        var workItems = MockDataProvider.GetTasks(3);
        var workItem = workItems[0];

        // Act
        var result = ProjectActivityPropertyValidator.ValidateRemoveWorkItem(workItem, workItems);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Errors.Count, Is.EqualTo(0));
    }
}