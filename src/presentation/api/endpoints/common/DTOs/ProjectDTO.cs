namespace api.endpoints.common.DTOs;

public record ProjectDTO
(
    string Id,
    string Title,
    string ContainedIn,
    string Description,
    string Status,
    string Priority,
    string[] TimeRange
);