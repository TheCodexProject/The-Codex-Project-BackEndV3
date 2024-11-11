namespace api.endpoints.common.DTOs;

public record UserDTO(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    List<string> OwnedOrganizations,    // * IDS of organizations owned by the user
    List<string> MemberOfOrganizations  // * IDS of organizations the user is a member of
);