using System;

namespace Model.Event
{
    public interface OrderSubmitted
    {
        Guid OrderId { get; }
    }
}