using domain.exceptions;
using domain.models.project;
using domain.shared;
using OperationResult;

namespace application.appEntry.commands.project;

public class UpdateProjectCommand
{
    public Guid Id { get; set; }

    public Project? Project { get; set; } = null;

    public string? Title { get; }
    public string? Description { get; }
    public Status? ProjectStatus { get; }
    public Priority? ProjectPriority { get; }
    public DateTime? Start { get; }
    public DateTime? End { get; }

    private UpdateProjectCommand(Guid id, string title, string description, Status projectStatus, Priority projectPriority, DateTime start, DateTime end)
    {
        Id = id;
        Title = title;
        Description = description;
        ProjectStatus = projectStatus;
        ProjectPriority = projectPriority;
        Start = start;
        End = end;
    }

    public static Result<UpdateProjectCommand> Create(string id, string? title, string? description, string? status, string? priority, string? start, string? end)
    {
        // ! Validate the user's input
        var validationResult = Validate(id, title, description, status, priority, start, end);

        // ? Were there any validation errors?
        if (validationResult.IsFailure)
            return Result<UpdateProjectCommand>.Failure(validationResult.Errors.ToArray());

        // * Convert start and end to DateTime
        var startDateTime = start !=  null ? DateTime.Parse(start) : DateTime.MinValue;
        var endDateTime = end != null ? DateTime.Parse(end) : DateTime.MinValue;
        if (!Enum.TryParse(status, true, out Status statusEnum))
        {
            statusEnum = Status.None;
        }

        if (!Enum.TryParse(priority, true, out Priority priorityEnum))
        {
            priorityEnum = Priority.None;
        }

        // * Return the newly created command.
        return new UpdateProjectCommand(new Guid(id), title, description, statusEnum, priorityEnum, startDateTime , endDateTime);
    }

    private static Result Validate(string id, string? title, string? description, string? status, string? priority, string? start, string? end)
    {
        List<Exception> exceptions = [];

        // ! Validate the ID
        if (!Guid.TryParse(id, out _))
            return Result.Failure(new FailedOperationException("The given ID could not be parsed into a GUID"));

        // ! Validate the title
        if (!string.IsNullOrWhiteSpace(title))
        {
            var titleValdiation = ProjectPropertyValidator.ValidateTitle(title);

            if (titleValdiation.IsFailure)
                exceptions.AddRange(titleValdiation.Errors);
        }

        // ! Validate the description
        if (!string.IsNullOrWhiteSpace(description))
        {
            var descriptionValidation = ProjectPropertyValidator.ValidateDescription(description);

            if (descriptionValidation.IsFailure)
                exceptions.AddRange(descriptionValidation.Errors);
        }

        // ! Validate the status
        if (!string.IsNullOrWhiteSpace(status))
        {
            // ? Is the status a valid enum value?
            if (!Enum.TryParse(typeof(Status), status, out _))
                // ! If not, return an error
                exceptions.Add(new FailedOperationException("The given status is not a valid status"));

            // * Convert the status to an enum
            var statusEnum = (Status)Enum.Parse(typeof(Status), status);

            var statusValidation = ProjectPropertyValidator.ValidateStatus(statusEnum);

            if (statusValidation.IsFailure)
                exceptions.AddRange(statusValidation.Errors);
        }

        // ! Validate the priority
        if (!string.IsNullOrWhiteSpace(priority))
        {
            // ? Is the priority a valid enum value?
            if (!Enum.TryParse(typeof(Priority), priority, out _))
                // ! If not, return an error
                exceptions.Add(new FailedOperationException("The given priority is not a valid priority"));

            // * Convert the priority to an enum
            var priorityEnum = (Priority)Enum.Parse(typeof(Priority), priority);

            var priorityValidation = ProjectPropertyValidator.ValidatePriority(priorityEnum);

            if (priorityValidation.IsFailure)
                exceptions.AddRange(priorityValidation.Errors);
        }

        // ! Validate the time range
        if (!string.IsNullOrWhiteSpace(start) && !string.IsNullOrWhiteSpace(end))
        {
            // ! Validate that the start and end dates are valid
            if (!DateTime.TryParse(start, out _) || !DateTime.TryParse(end, out _))
                exceptions.Add(new FailedOperationException("The given start or end date is not a valid date"));

            // ? If the start date is before the end date, the dates are valid then continue.
            var timeRangeValidation = ProjectPropertyValidator.ValidateTimeRange(DateTime.Parse(start),DateTime.Parse(end));

            if (timeRangeValidation.IsFailure)
                exceptions.AddRange(timeRangeValidation.Errors);
        }

        return exceptions.Count > 0
            ? Result.Failure(exceptions.ToArray())
            : Result.Success();
    }
}