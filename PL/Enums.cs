using System;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    // ===== DeliveryStatus =====
    public class DeliveryStatusCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.DeliveryStatus> _enums =
            (Enum.GetValues(typeof(BO.DeliveryStatus)) as IEnumerable<BO.DeliveryStatus>)!;

        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }

    // ===== OrderStatus =====
    public class OrderStatusCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.OrderStatus> _enums =
            (Enum.GetValues(typeof(BO.OrderStatus)) as IEnumerable<BO.OrderStatus>)!;

        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }

    // ===== CourierType =====
    public class CourierTypeCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.CourierType> _enums =
            (Enum.GetValues(typeof(BO.CourierType)) as IEnumerable<BO.CourierType>)!;

        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }

    // ===== DeliveryType =====
    public class DeliveryTypeCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.DeliveryType> _enums =
            (Enum.GetValues(typeof(BO.DeliveryType)) as IEnumerable<BO.DeliveryType>)!;

        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }

    public class ScheduleStatusCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.ScheduleStatus> _enums =
            (Enum.GetValues(typeof(BO.ScheduleStatus)) as IEnumerable<BO.ScheduleStatus>)!;

        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }
}
