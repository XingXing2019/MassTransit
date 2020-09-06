using System;
using MassTransit;

namespace Model.Event
{
    public interface OrderCanceled : CorrelatedBy<Guid>
    {
        Guid OrderId { get; }
    }
}