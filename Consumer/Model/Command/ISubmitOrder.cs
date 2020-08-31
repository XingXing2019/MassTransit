using System;
using MassTransit;

namespace Model.Command
{
    public interface ISubmitOrder : CorrelatedBy<Guid>
    {
        Guid CommandId { get; }
        int ClientId { get; }
    }
}