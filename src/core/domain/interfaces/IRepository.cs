namespace domain.interfaces;

public interface IRepository<T> where T : class
{
    /// <summary>
    /// A method to get all entities from the repository.
    /// </summary>
    /// <returns>All objects of type T stored in a database.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// A method to get an entity by its unique identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An object of type T</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// A method to add an toAdd to the repository.
    /// </summary>
    /// <param name="toAdd">An object of type T</param>
    /// <returns></returns>
    Task AddAsync(T toAdd);

    /// <summary>
    /// A method to update an toUpdate in the repository.
    /// </summary>
    /// <param name="toUpdate"></param>
    void Update(T toUpdate);

    /// <summary>
    /// A method to delete an toRemove from the repository.
    /// </summary>
    /// <param name="toRemove">An object of type T</param>
    void Remove(T toRemove);
}
