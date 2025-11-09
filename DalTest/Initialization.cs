namespace Dal;

using DalApi;
using DO;

/// <summary>
/// Handles the initialization of all data entities in the DAL system (Orders, Couriers, Deliveries, Config).
/// Used by DalTest to populate the data source with initial demo data for testing.
/// </summary>
public static class Initialization
{
    private static IDal? s_dal;


    /// <summary>
    /// Random generator used to create randomized demo data for initialization.
    /// </summary>
    private static readonly Random s_rand = new();

    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!");
        Console.WriteLine("Reset data...");
        s_dal.ResetDB();
        createCouriers();
        createOrders();
        createDeliveries();
    }


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
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating courier {i + 1}: {ex.Message}");
            }
        }
    }

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order {i + 1}: {ex.Message}");
            }
        }
    }

    private static void createDeliveries()
    {
        List<Order> orders = s_dal.Order.ReadAll();
        List<Courier> couriers = s_dal.Courier.ReadAll();

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
                s_dal.Delivery.Create(delivery);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating delivery {i + 1}: {ex.Message}");
            }
        }
    }
}
