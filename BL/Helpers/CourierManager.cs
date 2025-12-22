using DO;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helpers;

internal static class CourierManager
{
    private static readonly DalApi.IDal s_dal = DalApi.Factory.Get;

    internal static void Create(BO.Courier courier)
    {
        DO.Courier newDo = new()
        {
            Id = 0,
            FullName = courier.Name ?? "",
            Phone = courier.Phone,
            Email = courier.Email,
            Signature = courier.Signature,
            MaxPersonalDeliveryDistance = courier.MaxPersonalDeliveryDistance ?? 0,
            Type = (DO.DeliveryType)courier.Type,
            IsActive = courier.IsActive,
            StartWorkDate = courier.StartWorkDate
        };
        
        s_dal.Courier.Create(newDo);
    }

    internal static BO.Courier Get(int id)
    {
        DO.Courier? c = s_dal.Courier.Read(id);
        if (c == null) throw new BO.BlDoesNotExistException($"Courier with ID={id} not found");

        return new BO.Courier
        {
            Id = c.Id,
            Name = c.FullName,
            Phone = c.Phone,
            Email = c.Email,
            Signature = c.Signature,
            MaxPersonalDeliveryDistance = c.MaxPersonalDeliveryDistance,
            Type = (BO.DeliveryType)c.Type,
            IsActive = c.IsActive,
            StartWorkDate = (DateTime)c.StartWorkDate
        };
    }

    internal static IEnumerable<BO.Courier> ReadAll(Func<BO.Courier, bool>? filter = null)
    {
        var all = s_dal.Courier.ReadAll().Select(c => Get(c.Id));
        return filter == null ? all : all.Where(filter);
    }

    internal static void Update(BO.Courier courier)
    {
        DO.Courier? c = s_dal.Courier.Read(courier.Id);
        if (c == null) throw new BO.BlDoesNotExistException($"Courier with ID={courier.Id} not found");

        DO.Courier updated = c with
        {
            FullName = courier.Name ?? "",
            Phone = courier.Phone,
            Email = courier.Email,
            Signature = courier.Signature,
            MaxPersonalDeliveryDistance = courier.MaxPersonalDeliveryDistance ?? 0,
            Type = (DO.DeliveryType)courier.Type,
            IsActive = courier.IsActive,
            StartWorkDate = courier.StartWorkDate
        };

        s_dal.Courier.Update(updated);
    }

    internal static IEnumerable<BO.CourierInList> GetAll()
    {
        return s_dal.Courier.ReadAll()
            .Select(c => new BO.CourierInList
            {
                Id = c.Id,
                FullName = c.FullName,
                IsActive = c.IsActive
            });
    }

    internal static void Delete(int id) => s_dal.Courier.Delete(id);
    internal static void DeleteAll() => s_dal.Courier.DeleteAll();
}
