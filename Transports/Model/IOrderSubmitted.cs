using System;

namespace Model
{
    public interface IOrderSubmitted
    {
        public Guid OrderId { get; set; }
        public string OrderItem { get; set; }
    }
}