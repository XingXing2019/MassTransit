using System;
using MassTransit;

namespace Model
{
    // Command should be expressed in verb-noun, command should be sent (using Send) to an endpoint
    // If msg implement CorrelatedBY<Guid> interface, which has a Guid CorrelationId property, its values will be used as msg correlation_Id
    public interface IUpdateCustomerAddress : CorrelatedBy<Guid>
    {
        // If msg has a property named CorrelationId, CommandId or EventId that is a Guid or Guid?, its value will be used as msg correlation_id
        Guid CommandId { get; }

        DateTime Timestamp { get; }
        string CustomerId { get; }
    }
}