using System;
using MassTransit;

namespace Model
{
    public interface ISubmitOrder : CorrelatedBy<Guid>
    {
        Guid CommandId { get; }
        Guid OrderId { get; }
        DateTime OrderDate { get; }
        string OrderNumber { get; }
        IOrderItem[] OrderItems { get; }
    }
}