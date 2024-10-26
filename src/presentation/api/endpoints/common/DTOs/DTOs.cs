namespace api.endpoints.organization.models;

public class DTOs
{
    public record OrganizationDTO(string Id, string Name, UserDTO Owner, List<UserDTO> Members);

    public record UserDTO(string Id, string Name, string Email);

    public record WorkspaceDTO(string Id, string Title, string OwnedBy, List<DTOs.UserDTO> Contacts, List<string> Projects);
}