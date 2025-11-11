namespace Dal;

using DalApi;
using DO;

/// <summary>
/// Handles the initialization of all data entities in the DAL system (Orders, Couriers, Deliveries, Config).
/// This class is used to populate the DAL with initial demo data for testing purposes.
/// </summary>
public static class Initialization
{
    /// <summary>
    /// Reference to the DAL instance used for CRUD operations during initialization.
    /// </summary>
    private static IDal? s_dal;

    /// <summary>
    /// Random number generator used to create randomized demo data for initialization.
    /// </summary>
    private static readonly Random s_rand = new();

    /// <summary>
    /// Performs the initialization process:
    /// 1. Resets the database.
    /// 2. Creates demo Couriers.
    /// 3. Creates demo Orders.
    /// 4. Creates demo Deliveries.
    /// </summary>
    /// <param name="dal">DAL instance used to perform the initialization.</param>
    /// <exception cref="ArgumentNullException">Thrown if the provided DAL instance is null.</exception>
    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new ArgumentNullException(nameof(dal), "DAL object cannot be null!");
        Console.WriteLine("Reset data...");
        s_dal.ResetDB();
        createCouriers();
        createOrders();
        createDeliveries();
    }

    #region Private Initialization Methods

    /// <summary>
    /// Creates a set of demo Couriers in the DAL.
    /// Each courier has a unique name, email, phone, and other properties.
    /// Handles exceptions for duplicates or missing entities.
    /// </summary>
    private static void createCouriers()
    {
        for (int i = 0; i < 5; i++)
        {
            Courier courier = new(
                Id: 0,
                FullName: $"Courier {i + 1}",
                Phone: $"050-12{i}3456",
                Email: $"courier{i + 1}@mail.com",
                Signature: $"Sign{i + 1}",
                IsActive: true,
                MaxPersonalDeliveryDistance: 10 + i * 2,
                Type: (DeliveryType)(i % Enum.GetValues(typeof(DeliveryType)).Length),
                StartWorkDate: DateTime.Now.AddDays(-i * 30)
            );

            try
            {
                s_dal.Courier.Create(courier);
            }
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine($"Duplicate courier: {ex.Message}");
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine($"Missing entity: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Creates a set of demo Orders in the DAL.
    /// Each order has a type, description, address, customer information, weight, and open date.
    /// Handles exceptions for duplicates.
    /// </summary>
    private static void createOrders()
    {
        for (int i = 0; i < 8; i++)
        {
            Order order = new(
                Id: 0,
                Type: (OrderType)(i % Enum.GetValues(typeof(OrderType)).Length),
                Description: $"Order number {i + 1}",
                Address: $"Ben Yehuda {20 + i}, Tel Aviv",
                Latitude: 32.08 + (i * 0.001),
                Longitude: 34.78 + (i * 0.001),
                CustomerName: $"Customer {i + 1}",
                CustomerPhone: $"052-88{i}77{i}",
                Weight: 2 + i * 0.5,
                OpenDate: DateTime.Now.AddDays(-i)
            );

            try
            {
                s_dal.Order.Create(order);
            }
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine($"Order already exists: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error creating order {i + 1}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Creates a set of demo Deliveries in the DAL.
    /// Each delivery links an existing Order with an existing Courier.
    /// The delivery includes type, start/end dates, distances, and completion status.
    /// Handles exceptions for duplicates.
    /// </summary>
    private static void createDeliveries()
    {
        IEnumerable<Order> orders = s_dal.Order.ReadAll();
        IEnumerable<Courier> couriers = s_dal.Courier.ReadAll();

        int deliveriesCount = Math.Min(orders.Count(), couriers.Count());

        for (int i = 0; i < deliveriesCount; i++)
        {
            Delivery delivery = new(
                Id: 0,
                OrderId: orders.ElementAt(i).Id,
                CourierId: couriers.ElementAt(i).Id,
                Type: couriers.ElementAt(i).Type,
                StartDeliveryDate: DateTime.Now.AddDays(-i * 2),
                ActualDistance: 5 + i * 0.8,
                ExpectedDistance: 6 + i * 0.5,
                CompletionStatus: (i % 2 == 0) ? DeliveryStatus.Delivered : DeliveryStatus.InProgress,
                EndDeliveryDate: (i % 2 == 0) ? DateTime.Now.AddDays(-i) : null
            );

            try
            {
                s_dal.Delivery.Create(delivery);
            }
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine($"Delivery already exists: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error creating delivery {i + 1}: {ex.Message}");
            }
        }
    }

    #endregion
}
