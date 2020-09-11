using System;

namespace Model.Event
{
    public interface OrderNotFound
    {
        Guid OrderId { get; }
    }
}