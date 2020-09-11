using System;

namespace Model.Command
{
    public class UpdateAccountHistoryCommand : UpdateAccountHistory
    {
        public UpdateAccountHistoryCommand(Guid orderId)
        {
            OrderId = orderId;
        }
        public Guid OrderId { get; }
    }
}