using System;
using MassTransit;

namespace Model.Event
{
    public interface IOrderSubmitted : CorrelatedBy<Guid>
    {
        Guid EventId { get; }
        int ClientId { get; }
    }
}