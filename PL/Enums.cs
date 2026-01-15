using System;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    /// <summary>
    /// Provides an enumerable collection of delivery status values
    /// for data binding and UI usage.
    /// </summary>
    public class DeliveryStatusCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.DeliveryStatus> _enums =
            (Enum.GetValues(typeof(BO.DeliveryStatus)) as IEnumerable<BO.DeliveryStatus>)!;

        /// <summary>
        /// Returns an enumerator over all delivery status values.
        /// </summary>
        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }

    /// <summary>
    /// Provides an enumerable collection of order status values
    /// for data binding and UI usage.
    /// </summary>
    public class OrderStatusCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.OrderStatus> _enums =
            (Enum.GetValues(typeof(BO.OrderStatus)) as IEnumerable<BO.OrderStatus>)!;

        /// <summary>
        /// Returns an enumerator over all order status values.
        /// </summary>
        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }

    /// <summary>
    /// Provides an enumerable collection of courier type values
    /// for data binding and UI usage.
    /// </summary>
    public class CourierTypeCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.CourierType> _enums =
            (Enum.GetValues(typeof(BO.CourierType)) as IEnumerable<BO.CourierType>)!;

        /// <summary>
        /// Returns an enumerator over all courier type values.
        /// </summary>
        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }

    /// <summary>
    /// Provides an enumerable collection of delivery type values
    /// for data binding and UI usage.
    /// </summary>
    public class DeliveryTypeCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.DeliveryType> _enums =
            (Enum.GetValues(typeof(BO.DeliveryType)) as IEnumerable<BO.DeliveryType>)!;

        /// <summary>
        /// Returns an enumerator over all delivery type values.
        /// </summary>
        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }

    /// <summary>
    /// Provides an enumerable collection of schedule status values
    /// for data binding and UI usage.
    /// </summary>
    public class ScheduleStatusCollection : IEnumerable
    {
        private static readonly IEnumerable<BO.ScheduleStatus> _enums =
            (Enum.GetValues(typeof(BO.ScheduleStatus)) as IEnumerable<BO.ScheduleStatus>)!;

        /// <summary>
        /// Returns an enumerator over all schedule status values.
        /// </summary>
        public IEnumerator GetEnumerator() => _enums.GetEnumerator();
    }
}
