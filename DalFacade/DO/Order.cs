namespace DO;

/// <summary>
/// Order entity represents a delivery order placed by a customer.
/// Contains all relevant data such as customer details, location, weight, and creation time.
/// </summary>
/// <param name="Id">Unique ID of the order (as defined by the running order number in Config).</param>
/// <param name="Type">Type of order (Regular, Express, SameDay).</param>
/// <param name="Description">Short description of the order contents.</param>
/// <param name="Address">Full delivery address in standard format.</param>
/// <param name="Latitude">Latitude coordinate (X) of the order address.</param>
/// <param name="Longitude">Longitude coordinate (Y) of the order address.</param>
/// <param name="CustomerName">Full name of the customer who placed the order.</param>
/// <param name="CustomerPhone">Customer’s phone number (10 digits, numeric only).</param>
/// <param name="Weight">Order’s weight in kilograms (positive double).</param>
/// <param name="OpenDate">Date and time when the order was opened in the system.</param>
public record Order(
    int Id,
    OrderType Type,
    string? Description,
    string Address,
    double? Latitude = null,
    double? Longitude = null,
    string CustomerName = "",
    string CustomerPhone = "",
    double Weight = 0,
    DateTime? OpenDate = null
)
{
    /// <summary>
    /// Default constructor initializing an empty order
    /// with default values for all fields.
    /// </summary>
    public Order() : this(0, default, "", "", 0, 0, "", "", 0, default) { }
}
