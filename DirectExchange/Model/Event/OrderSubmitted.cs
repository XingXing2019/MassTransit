using System;

namespace Model.Event
{
    public class OrderSubmitted : IOrderSubmitted
    {
        public Guid CorrelationId { get; set; }
        public Guid EventId { get; set; }
        public string OrderType { get; set; }
    }
}