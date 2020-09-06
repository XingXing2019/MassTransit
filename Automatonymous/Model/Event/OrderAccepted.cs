using System;
using MassTransit;

namespace Model.Event
{
    public interface OrderAccepted : CorrelatedBy<Guid>
    {
        Guid OrderId { get; }
    }
}