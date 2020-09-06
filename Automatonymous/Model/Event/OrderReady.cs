using System;
using MassTransit;

namespace Model.Event
{
    public interface OrderReady : CorrelatedBy<Guid>
    {
        Guid OrderId { get; }
    }
}