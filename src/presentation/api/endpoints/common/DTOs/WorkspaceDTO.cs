namespace api.endpoints.common.DTOs;

public record WorkspaceDTO(
    string Id,
    string Title,
    string OwnedBy,
    List<UserDTO> Contacts,
    List<string> Projects
);
