using System;
using MassTransit;

namespace Model.Command
{
    public interface SubmitOrder : CorrelatedBy<Guid>
    {
        Guid OrderId { get; }
        public DateTime OrderDate { get; set; }
    }
}