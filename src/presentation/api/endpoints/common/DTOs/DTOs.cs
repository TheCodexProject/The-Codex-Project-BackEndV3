namespace api.endpoints.common.DTOs;

public class DTOs
{
    public record ResourceDTO(string Id, string Title, string Description, string URL, string Type);

    public record ActivityDTO(string Id, string ContainedIn, string Title, string Description,  List<string> Items);
}