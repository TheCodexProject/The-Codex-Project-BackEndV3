namespace api.endpoints.common.DTOs;

public record WorkItemDTO(
    string Id,
    string ContainedIn,
    string Title,
    string Description,
    string Status,
    string Priority,
    string Type,
    string AssignedTo
    );