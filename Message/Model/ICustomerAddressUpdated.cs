using System;
using MassTransit;

namespace Model
{
    public interface ICustomerAddressUpdated : CorrelatedBy<Guid>
    {
        Guid EventId { get; }

        DateTime Timestamp { get; }
        string CustomerId { get; }
    }
}