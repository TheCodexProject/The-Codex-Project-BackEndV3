﻿namespace api.endpoints.common.DTOs;

public class DTOs
{
    public record OrganizationDTO(string Id, string Name, UserDTO Owner, List<UserDTO> Members);

    public record UserDTO(string Id, string Name, string Email);

    public record WorkspaceDTO(string Id, string Title, string OwnedBy, List<DTOs.UserDTO> Contacts, List<string> Projects);

    public record ProjectDTO(string Id, string Title, string Description, string Status, string Priority, string[] TimeRange, string ContainedIn);

    public record WorkItemDTO(string Id,string ContainedIn, string Title, string Description, string Status, string Priority, string Type, string AssignedTo);

    public record ResourceDTO(string Id, string Title, string Description, string URL, string Type);

    public record ActivityDTO(string Id, string ContainedIn, string Title, string Description,  List<string> Items);
}