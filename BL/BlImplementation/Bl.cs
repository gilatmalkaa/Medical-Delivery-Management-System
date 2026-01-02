namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    public ICourier Couriers { get; } = new CourierImplementation();
    public IOrder Orders { get; } = new OrderImplementation();
    public IDelivery Deliveries { get; } = new DeliveryImplementation();
    public IAdmin Admin { get; } = new AdminImpnamespace BlImplementation;
using BlApi;

/// <summary>
/// Central Business Logic container.
/// Exposes BL services via interfaces.
/// </summary>
internal class Bl : IBl
    {
        public ICourier Couriers { get; } = new CourierImplementation();
        public IOrder Orders { get; } = new OrderImplementation();
        public IDelivery Deliveries { get; } = new DeliveryImplementation();
        public IAdmin Admin { get; } = new AdminImplementation();
    }
    lementation();
}
