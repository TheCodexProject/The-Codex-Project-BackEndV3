using domain.models.project;
using domain.models.projectActivity.value;
using OperationResult;

namespace domain.models.projectActivity;

/// <summary>
/// The builder for the ProjectActivity class. (Allows for easier different types of project activities.)
/// </summary>
public static class ProjectActivityBuilder
{
    /// <summary>
    /// Builds a milestone for the project.
    /// </summary>
    /// <param name="belongsTo">The project the milestone should belong to.</param>
    /// <param name="title">The title of the milestone itself.</param>
    /// <returns> Returns a <see cref="Result"/> with the created milestone or a list of errors for the user to fix.</returns>
    public static Result<ProjectActivity> BuildMilestone(Project belongsTo, string title)
    {
        // * Create a new milestone.
        var milestone = ProjectActivity.Create(belongsTo, title, ProjectActivityType.Milestone);

        // ? Were there any errors during the creation of the milestone?
        return milestone.IsFailure ?
            // ! Return the errors
            Result<ProjectActivity>.Failure(milestone.Errors.ToArray()) : milestone;
    }

    /// <summary>
    /// Builds an iteration for the project.
    /// </summary>
    /// <param name="belongsTo">The project the iteration should belong to.</param>
    /// <param name="title">The title of the iteration itself.</param>
    /// <returns> Returns a <see cref="Result"/> with the created iteration or a list of errors for the user to fix.</returns>
    public static Result<ProjectActivity> BuildIteration(Project belongsTo, string title)
    {
        // * Create a new iteration.
        var iteration = ProjectActivity.Create(belongsTo, title, ProjectActivityType.Iteration);

        // ? Were there any errors during the creation of the iteration?
        return iteration.IsFailure ?
            // ! Return the errors
            Result<ProjectActivity>.Failure(iteration.Errors.ToArray()) : iteration;
    }
}