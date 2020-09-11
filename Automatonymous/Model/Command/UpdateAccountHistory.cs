using System;

namespace Model.Command
{
    public interface UpdateAccountHistory
    {
        Guid OrderId { get; }
    }
}