namespace api.endpoints.common.DTOs;

public record ResourceDTO
(
    string Id,
    string Title,
    string Description,
    string Url,
    string Type
);
