using DalApi;
using Dal;
using DO;

namespace DalTest
{
    /// <summary>
    /// Console-based test program for the DAL (Data Access Layer).
    /// Provides interactive menus to test CRUD operations for Orders, Couriers, Deliveries, and Config.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// DAL instance used for performing all CRUD operations.
        /// </summary>
        // static readonly IDal s_dal = new DalList(); //stage 2  (תשאיר בהערה)
        static readonly IDal s_dal = new DalXml(); //stage 3

        /// <summary>
        /// Main entry point of the program.
        /// Initializes the DAL, handles exceptions during initialization, 
        /// and launches the main menu loop.
        /// </summary>
        static void Main()
        {
            try
            {
                Initialization.Do(s_dal);
                Console.WriteLine("Initialization completed successfully!");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Initialization failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected initialization error: {ex.Message}");
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

        #region Orders Menu

        /// <summary>
        /// Displays and handles the Orders menu, providing options to:
        /// Add, Read, ReadAll, and Delete Orders.
        /// </summary>
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
                        AddOrder();
                        break;

                    case 2:
                        ShowOrderById();
                        break;

                    case 3:
                        ShowAllOrders();
                        break;

                    case 4:
                        DeleteOrder();
                        break;

                    case 0:
                        return;
                }
            }
        }

        /// <summary>
        /// Prompts user for order details and attempts to add a new order to DAL.
        /// Handles exceptions such as already existing order.
        /// </summary>
        private static void AddOrder()
        {
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
            try
            {
                s_dal!.Order.Create(order);
                Console.WriteLine("Order added successfully!");
            }
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine($"Order already exists: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Prompts the user for an Order ID and displays the corresponding order.
        /// </summary>
        private static void ShowOrderById()
        {
            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine()!);
            try
            {
                var order = s_dal!.Order.Read(id);
                Console.WriteLine(order);
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine($"Order not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays all orders stored in DAL.
        /// </summary>
        private static void ShowAllOrders()
        {
            try
            {
                var orders = s_dal!.Order.ReadAll();
                if (!orders.Any())
                    Console.WriteLine("No orders found.");
                else
                    foreach (var o in orders)
                        Console.WriteLine(o);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading orders: {ex.Message}");
            }
        }

        /// <summary>
        /// Prompts the user for an Order ID and attempts to delete it from DAL.
        /// </summary>
        private static void DeleteOrder()
        {
            Console.Write("Enter ID to delete: ");
            int delId = int.Parse(Console.ReadLine()!);
            try
            {
                s_dal!.Order.Delete(delId);
                Console.WriteLine("Order deleted successfully!");
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine($"Cannot delete order: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        #endregion

        #region Couriers Menu

        /// <summary>
        /// Displays and handles the Couriers menu, providing options to:
        /// Add, Read, ReadAll, and Delete Couriers.
        /// </summary>
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
                        AddCourier();
                        break;
                    case 2:
                        ShowCourierById();
                        break;
                    case 3:
                        ShowAllCouriers();
                        break;
                    case 4:
                        DeleteCourier();
                        break;
                    case 0:
                        return;
                }
            }
        }

        /// <summary>
        /// Prompts user for courier details and attempts to add a new courier to DAL.
        /// </summary>
        private static void AddCourier()
        {
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

            try
            {
                s_dal!.Courier.Create(courier);
                Console.WriteLine("Courier added successfully!");
            }
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine($"Courier already exists: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Prompts the user for a Courier ID and displays the corresponding courier.
        /// </summary>
        private static void ShowCourierById()
        {
            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine()!);
            try
            {
                var courier = s_dal!.Courier.Read(id);
                Console.WriteLine(courier);
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine($"Courier not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays all couriers stored in DAL.
        /// </summary>
        private static void ShowAllCouriers()
        {
            try
            {
                var couriers = s_dal!.Courier.ReadAll();
                if (!couriers.Any())
                    Console.WriteLine("No couriers found.");
                else
                    foreach (var c in couriers)
                        Console.WriteLine(c);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading couriers: {ex.Message}");
            }
        }

        /// <summary>
        /// Prompts the user for a Courier ID and attempts to delete it from DAL.
        /// </summary>
        private static void DeleteCourier()
        {
            Console.Write("Enter ID to delete: ");
            int delId = int.Parse(Console.ReadLine()!);
            try
            {
                s_dal!.Courier.Delete(delId);
                Console.WriteLine("Courier deleted successfully!");
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine($"Cannot delete courier: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        #endregion

        #region Deliveries Menu

        /// <summary>
        /// Displays and handles the Deliveries menu, providing options to:
        /// Add, Read, ReadAll, and Delete Deliveries.
        /// </summary>
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
                        AddDelivery();
                        break;
                    case 2:
                        ShowDeliveryById();
                        break;
                    case 3:
                        ShowAllDeliveries();
                        break;
                    case 4:
                        DeleteDelivery();
                        break;
                    case 0:
                        return;
                }
            }
        }

        /// <summary>
        /// Prompts user for Order ID and Courier ID to create a new Delivery in DAL.
        /// </summary>
        private static void AddDelivery()
        {
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

            try
            {
                s_dal!.Delivery.Create(delivery);
                Console.WriteLine("Delivery added successfully!");
            }
            catch (DalAlreadyExistsException ex)
            {
                Console.WriteLine($"Delivery already exists: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Prompts user for Delivery ID and displays the corresponding delivery.
        /// </summary>
        private static void ShowDeliveryById()
        {
            Console.Write("Enter ID: ");
            int id = int.Parse(Console.ReadLine()!);
            try
            {
                var delivery = s_dal!.Delivery.Read(id);
                Console.WriteLine(delivery);
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine($"Delivery not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays all deliveries stored in DAL.
        /// </summary>
        private static void ShowAllDeliveries()
        {
            try
            {
                var deliveries = s_dal!.Delivery.ReadAll();
                if (!deliveries.Any())
                    Console.WriteLine("No deliveries found.");
                else
                    foreach (var d in deliveries)
                        Console.WriteLine(d);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading deliveries: {ex.Message}");
            }
        }

        /// <summary>
        /// Prompts the user for a Delivery ID and attempts to delete it from DAL.
        /// </summary>
        private static void DeleteDelivery()
        {
            Console.Write("Enter ID to delete: ");
            int delId = int.Parse(Console.ReadLine()!);
            try
            {
                s_dal!.Delivery.Delete(delId);
                Console.WriteLine("Delivery deleted successfully!");
            }
            catch (DalDoesNotExistException ex)
            {
                Console.WriteLine($"Cannot delete delivery: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        #endregion

        #region Config Menu

        /// <summary>
        /// Displays and handles the Config menu, currently providing option to reset configuration.
        /// </summary>
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
                        ResetConfig();
                        break;
                    case 0:
                        return;
                }
            }
        }

        /// <summary>
        /// Resets the DAL configuration to default values.
        /// </summary>
        private static void ResetConfig()
        {
            try
            {
                s_dal!.Config.Reset();
                Console.WriteLine("Config Reset successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting config: {ex.Message}");
            }
        }

        #endregion
    }
}
