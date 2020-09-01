using System;

namespace Model
{
    public interface IOrderItem
    {
        Guid OrderId { get; }
        string OrderNumber { get; }
    }
}