using DalApi;
using Dal;
using DO;


namespace DalTest;

/// <summary>
/// Console-based test program for the DAL layer.
/// Allows CRUD operations testing for Orders, Couriers, Deliveries, and Config.
/// </summary>
internal class Program
{
    static readonly IDal s_dal = new DalList();
    static void Main()
    {
        try
        {
            Initialization.Do(s_dal);
            Console.WriteLine("Initialization completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Initialization error: {ex.Message}");
        }

        while (true)
        {
            Console.WriteLine("\n=== DAL Test Menu ===");
            Console.WriteLine("1. Orders");
            Console.WriteLine("2. Couriers");
            Console.WriteLine("3. Deliveries");
            Console.WriteLine("4. Config");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                continue;

            try
            {
                switch (choice)
                {
                    case 1: OrderMenu(); break;
                    case 2: CourierMenu(); break;
                    case 3: DeliveryMenu(); break;
                    case 4: ConfigMenu(); break;
                    case 0: return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // === Orders Menu ===
    private static void OrderMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Orders Menu ---");
            Console.WriteLine("1. Add Order");
            Console.WriteLine("2. Show Order by ID");
            Console.WriteLine("3. Show All Orders");
            Console.WriteLine("4. Delete Order");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    var order = new Order(
                        Id: 0,
                        Type: OrderType.Regular,
                        Description: "Test order",
                        Address: "Tel Aviv 10",
                        Latitude: 32.07,
                        Longitude: 34.78,
                        CustomerName: "Test Customer",
                        CustomerPhone: "0521234567",
                        Weight: 2.5,
                        OpenDate: DateTime.Now
                    );
                    s_dal!.Order.Create(order);
                    Console.WriteLine("Order added successfully!");
                    break;
                case 2:
                    Console.Write("Enter ID: ");
                    int id = int.Parse(Console.ReadLine()!);
                    Console.WriteLine(s_dal!.Order.Read(id));
                    break;

                case 3:
                    foreach (var o in s_dal!.Order.ReadAll())
                        Console.WriteLine(o);
                    break;

                case 4:
                    Console.Write("Enter ID to delete: ");
                    int delId = int.Parse(Console.ReadLine()!);
                    s_dal!.Order.Delete(delId);
                    Console.WriteLine("Order deleted.");
                    break;

                case 0:
                    return;
            }
        }
    }

    // === Couriers Menu ===
    private static void CourierMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Couriers Menu ---");
            Console.WriteLine("1. Add Courier");
            Console.WriteLine("2. Show Courier by ID");
            Console.WriteLine("3. Show All Couriers");
            Console.WriteLine("4. Delete Courier");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    var courier = new Courier(
                        Id: 0,
                        FullName: "Test Courier",
                        Phone: "0501234567",
                        Email: "courier@test.com",
                        Signature: "Sig",
                        IsActive: true,
                        MaxPersonalDeliveryDistance: 15,
                        Type: DeliveryType.Foot,
                        StartWorkDate: DateTime.Now
                    );
                    s_dal!.Courier.Create(courier);
                    Console.WriteLine("Courier added successfully!");
                    break;

                case 2:
                    Console.Write("Enter ID: ");
                    int id = int.Parse(Console.ReadLine()!);
                    Console.WriteLine(s_dal!.Courier.Read(id));
                    break;

                case 3:
                    foreach (var c in s_dal!.Courier.ReadAll())
                        Console.WriteLine(c);
                    break;

                case 4:
                    Console.Write("Enter ID to delete: ");
                    int delId = int.Parse(Console.ReadLine()!);
                    s_dal!.Courier.Delete(delId);
                    Console.WriteLine("Courier deleted.");
                    break;

                case 0:
                    return;
            }
        }
    }

    // === Deliveries Menu ===
    private static void DeliveryMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Deliveries Menu ---");
            Console.WriteLine("1. Add Delivery");
            Console.WriteLine("2. Show Delivery by ID");
            Console.WriteLine("3. Show All Deliveries");
            Console.WriteLine("4. Delete Delivery");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");
            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

            switch (choice)
            {
                case 1:
                    Console.Write("Enter Order ID: ");
                    int orderId = int.Parse(Console.ReadLine()!);
                    Console.Write("Enter Courier ID: ");
                    int courierId = int.Parse(Console.ReadLine()!);

                    var delivery = new Delivery(
                        Id: 0,
                        OrderId: orderId,
                        CourierId: courierId,
                        Type: DeliveryType.Foot,
                        StartDeliveryDate: DateTime.Now,
                        ActualDistance: 5.0,
                        ExpectedDistance: 6.0,
                        CompletionStatus: DeliveryStatus.InProgress,
                        EndDeliveryDate: null
                    );
                    s_dal!.Delivery.Create(delivery);
                    Console.WriteLine("Delivery added successfully!");
                    break;

                case 2:
                    Console.Write("Enter ID: ");
                    int id = int.Parse(Console.ReadLine()!);
                    Console.WriteLine(s_dal!.Delivery.Read(id));
                    break;

                case 3:
                    foreach (var d in s_dal!.Delivery.ReadAll())
                        Console.WriteLine(d);
                    break;

                case 4:
                    Console.Write("Enter ID to delete: ");
                    int delId = int.Parse(Console.ReadLine()!);
                    s_dal!.Delivery.Delete(delId);
                    Console.WriteLine("Delivery deleted.");
                    break;

                case 0:
                    return;
            }
        }
    }

    // === Config Menu ===
    private static void ConfigMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Config Menu ---");
            Console.WriteLine("1. Reset Config");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                continue;

            switch (choice)
            {
                case 1:
                    s_dal!.Config.Reset();
                    Console.WriteLine("Config Reset successfully.");
                    break;
                case 0:
                    return;
            }
        }
    }
}
