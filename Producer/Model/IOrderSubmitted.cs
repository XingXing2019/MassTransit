using System;
using MassTransit;

namespace Model
{
    public interface IOrderSubmitted : CorrelatedBy<Guid>
    {
        Guid CommandId { get; }
        Guid OrderId { get; }
        DateTime OrderDate { get; }
        string OrderNumber { get; }
        IOrderItem[] OrderItems { get; }
    }
}