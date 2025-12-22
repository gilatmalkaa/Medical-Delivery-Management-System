using DalApi;

namespace BlApi;

public interface IBl
{
    ICourier Couriers { get; }
    IOrder Orders { get; }
    IDelivery Deliveries { get; }
    IAdmin Admin { get; }
}