using domain.models.projectActivity;
using domain.models.projectActivity.value;
using unitTests.utils;

namespace unitTests.models.projectActivity;

[TestFixture]
public class ProjectActivityBuilderTests
{

    // SECTION #1: Build a milestone

    [Test]
    public void Build_A_Milestone_Should_Return_A_Valid_Milestone()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "Milestone 1";

        // Act
        var result = ProjectActivityBuilder.BuildMilestone(project, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.ProjectId, Is.EqualTo(project.Id));
            Assert.That(result.Value.Title, Is.EqualTo(title));
            Assert.That(result.Value.Type, Is.EqualTo(ProjectActivityType.Milestone));
        });
    }

    // SECTION #2: Build an iteration

    [Test]
    public void Build_An_Iteration_Should_Return_A_Valid_Iteration()
    {
        // Arrange
        var project = MockDataProvider.GetProject();
        const string title = "Iteration 1";

        // Act
        var result = ProjectActivityBuilder.BuildIteration(project, title);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.ProjectId, Is.EqualTo(project.Id));
            Assert.That(result.Value.Title, Is.EqualTo(title));
            Assert.That(result.Value.Type, Is.EqualTo(ProjectActivityType.Iteration));
        });
    }

}