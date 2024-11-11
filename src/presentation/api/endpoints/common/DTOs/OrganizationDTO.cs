namespace api.endpoints.common.DTOs;

public record OrganizationDTO
(
    string Id,
    string Name,
    UserDTO Owner,
    List<string> Members // * Member IDs for the organization.
);