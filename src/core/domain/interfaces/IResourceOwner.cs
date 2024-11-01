using domain.models.resource;
using OperationResult;

namespace domain.interfaces;

/// <summary>
/// An interface to indicate that the implementing class can own resources.
/// </summary>
public interface IResourceOwner
{
    public List<Resource> Resources { get; }
    public Result AddResource(string title, string url);

    public Result RemoveResource(Resource resource);
}