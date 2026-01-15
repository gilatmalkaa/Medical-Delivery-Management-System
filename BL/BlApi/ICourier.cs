namespace BlApi;

/// <summary>
/// Business logic service for managing couriers.
/// Provides CRUD operations, availability handling,
/// order assignment, and observer support.
/// </summary>
public interface ICourier : IObservable
{
    /// <summary>
    /// Creates a new courier in the system.
    /// </summary>
    /// <param name="courier">Courier business object to create.</param>
    void Create(BO.Courier courier);

    /// <summary>
    /// Retrieves a courier by its unique identifier.
    /// </summary>
    /// <param name="id">Courier ID.</param>
    /// <returns>The corresponding courier object.</returns>
    BO.Courier Get(int id);

    /// <summary>
    /// Retrieves all couriers, optionally filtered by a predicate.
    /// </summary>
    /// <param name="filter">
    /// Optional filter function applied to each courier.
    /// </param>
    /// <returns>A collection of couriers.</returns>
    IEnumerable<BO.Courier> ReadAll(Func<BO.Courier, bool>? filter = null);

    /// <summary>
    /// Updates an existing courier's details.
    /// </summary>
    /// <param name="courier">Courier object containing updated data.</param>
    void Update(BO.Courier courier);

    /// <summary>
    /// Deletes a courier by its identifier.
    /// </summary>
    /// <param name="id">Courier ID.</param>
    void Delete(int id);

    /// <summary>
    /// Assigns an open order to the specified courier.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    /// <param name="orderId">Order identifier.</param>
    void AssignOrder(int courierId, int orderId);

    /// <summary>
    /// Marks the courier's active delivery as completed.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    void CompleteDelivery(int courierId);

    /// <summary>
    /// Retrieves a summarized list of couriers for display purposes.
    /// </summary>
    /// <returns>A collection of courier summary objects.</returns>
    IEnumerable<BO.CourierInList> GetAll();

    /// <summary>
    /// Returns all open orders that the specified courier
    /// is eligible to choose from.
    /// </summary>
    /// <param name="courierId">Courier identifier.</param>
    /// <returns>A collection of open orders available to the courier.</returns>
    IEnumerable<BO.OpenOrderInList> GetOpenOrdersForCourier(int courierId);
}

