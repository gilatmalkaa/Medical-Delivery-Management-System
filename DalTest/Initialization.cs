namespace Dal;

using DalApi;
using DO;

/// <summary>
/// Handles the initialization of all data entities in the DAL system (Orders, Couriers, Deliveries, Config).
/// Used by DalTest to populate the data source with initial demo data for testing.
/// </summary>
public static class Initialization
{
    /// <summary>
    /// DAL interface for handling Orders.
    /// </summary>
    private static IOrder? s_dalOrder;

    /// <summary>
    /// DAL interface for handling Couriers.
    /// </summary>
    private static ICourier? s_dalCourier;

    /// <summary>
    /// DAL interface for handling Deliveries.
    /// </summary>
    private static IDelivery? s_dalDelivery;

    /// <summary>
    /// DAL interface for handling Config (configuration values).
    /// </summary>
    private static IConfig? s_dalConfig;

    /// <summary>
    /// Random generator used to create randomized demo data for initialization.
    /// </summary>
    private static readonly Random s_rand = new();


    /// <summary>
    /// Creates a collection of demo couriers and adds them to the DAL.
    /// Used to populate the system with example Courier entities.
    /// </summary>
    private static void createCouriers()
    {
        for (int i = 0; i < 5; i++)
        {
            Courier courier = new(
                Id: 0, // ID generated automatically via NextCourierId
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
                s_dalCourier!.Create(courier);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating courier {i + 1}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Creates a collection of demo orders and adds them to the DAL.
    /// Used to populate the system with example Order entities.
    /// </summary>
    private static void createOrders()
    {
        for (int i = 0; i < 8; i++)
        {
            Order order = new(
                Id: 0, // Automatically assigned by Create()
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
                s_dalOrder!.Create(order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order {i + 1}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Creates demo deliveries connecting couriers to existing orders.
    /// Used to create relational data between Orders and Couriers.
    /// </summary>
    private static void createDeliveries()
    {
        List<Order> orders = s_dalOrder!.ReadAll().Where(o => o != null).Cast<Order>().ToList();
        List<Courier> couriers = s_dalCourier!.ReadAll().Where(c => c != null).Cast<Courier>().ToList();

        int deliveriesCount = Math.Min(orders.Count, couriers.Count);

        for (int i = 0; i < deliveriesCount; i++)
        {
            Delivery delivery = new(
                Id: 0,
                OrderId: orders[i].Id,
                CourierId: couriers[i].Id,
                Type: couriers[i].Type,
                StartDeliveryDate: DateTime.Now.AddDays(-i * 2),
                ActualDistance: 5 + i * 0.8,
                ExpectedDistance: 6 + i * 0.5,
                CompletionStatus: (i % 2 == 0) ? DeliveryStatus.Delivered : DeliveryStatus.InProgress,
                EndDeliveryDate: (i % 2 == 0) ? DateTime.Now.AddDays(-i) : null
            );

            try
            {
                s_dalDelivery!.Create(delivery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating delivery {i + 1}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Fully initializes all DAL entities.
    /// Steps performed:
    /// 1. Resets configuration values.
    /// 2. Clears all existing data in Orders, Couriers, and Deliveries.
    /// 3. Creates demo data for each entity type.
    /// </summary>
    /// <param name="dalOrder">Order DAL instance used for CRUD operations.</param>
    /// <param name="dalCourier">Courier DAL instance used for CRUD operations.</param>
    /// <param name="dalDelivery">Delivery DAL instance used for CRUD operations.</param>
    /// <param name="dalConfig">Config DAL instance used for system configuration.</param>
    public static void Do(IOrder dalOrder, ICourier dalCourier, IDelivery dalDelivery, IConfig dalConfig)
    {
        // === Validate input ===
        s_dalOrder = dalOrder ?? throw new NullReferenceException("DAL object cannot be null!");
        s_dalCourier = dalCourier ?? throw new NullReferenceException("DAL object cannot be null!");
        s_dalDelivery = dalDelivery ?? throw new NullReferenceException("DAL object cannot be null!");
        s_dalConfig = dalConfig ?? throw new NullReferenceException("DAL object cannot be null!");

        // === Reset & Clear ===
        Console.WriteLine("Reset configuration and clear data...");
        s_dalConfig.Reset();
        s_dalOrder.DeleteAll();
        s_dalCourier.DeleteAll();
        s_dalDelivery.DeleteAll();

        // === Initialize Entities ===
        Console.WriteLine("Creating Couriers...");
        createCouriers();
        Console.WriteLine("Creating Orders...");
        createOrders();
        Console.WriteLine("Creating Deliveries...");
        createDeliveries();

        Console.WriteLine("✅ Initialization completed successfully!");
    }
}
