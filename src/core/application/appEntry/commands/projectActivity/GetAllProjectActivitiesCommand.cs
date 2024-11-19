using domain.exceptions;
using domain.models.projectActivity;
using domain.models.projectActivity.value;
using OperationResult;

namespace application.appEntry.commands.projectActivity;

public class GetAllProjectActivitiesCommand
{
    // NOTE: Given information
    public Guid ProjectId { get; set; }
    public ProjectActivityType Type { get; set; }

    // NOTE: Result information
    public List<ProjectActivity> ProjectActivities { get; set; } = [];

    private GetAllProjectActivitiesCommand(Guid projectId, ProjectActivityType type)
    {
        ProjectId = projectId;
        Type = type;
    }

    public static Result<GetAllProjectActivitiesCommand> Create(string projectId, ProjectActivityType type)
    {
        // ? Is the ID valid?
        if (!Guid.TryParse(projectId, out var parsedProjectId))
            return Result<GetAllProjectActivitiesCommand>.Failure(new FailedOperationException("The given Project ID could not be parsed into a GUID"));

        return new GetAllProjectActivitiesCommand(new Guid(projectId), type);
    }
}