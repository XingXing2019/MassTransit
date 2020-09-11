using System;

namespace Model.Event
{
    public class OrderSubmittedEvent : OrderSubmitted  
    {
        public OrderSubmittedEvent(Guid orderId)
        {
            OrderId = orderId;
        }
        public Guid OrderId { get; }
    }
}