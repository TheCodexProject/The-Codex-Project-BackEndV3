namespace api.endpoints.common.DTOs;

public class DTOs
{
    public record ActivityDTO(string Id, string ContainedIn, string Title, string Description,  List<string> Items);
}