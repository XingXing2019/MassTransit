using System;

namespace Model.Command
{
    public interface RequestOrderCancellation
    {
        Guid OrderId { get; }
    }
}