using DO;

namespace DalApi;

/// <summary>
/// Defines generic CRUD operations for data access objects,
/// supporting creation, retrieval, update, and deletion.
/// </summary>
public interface ICrud<T> where T : class
{
    /// <summary>
    /// Creates a new entity in the data source.
    /// </summary>
    void Create(T item);

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    T? Read(int id);

    /// <summary>
    /// Retrieves a single entity that matches the given predicate.
    /// </summary>
    T? Read(Func<T, bool> filter);

    /// <summary>
    /// Retrieves all entities, optionally filtered by a predicate.
    /// </summary>
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null);

    /// <summary>
    /// Updates an existing entity in the data source.
    /// </summary>
    void Update(T item);

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    void Delete(int id);

    /// <summary>
    /// Deletes all entities from the data source.
    /// </summary>
    void DeleteAll();
}
