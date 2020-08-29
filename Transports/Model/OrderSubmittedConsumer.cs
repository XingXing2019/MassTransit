using System;
using System.Threading.Tasks;
using MassTransit;

namespace Model
{
    public class OrderSubmittedConsumer : IConsumer<IOrderSubmitted>
    {
        public async Task Consume(ConsumeContext<IOrderSubmitted> context)
        {
            await Console.Out.WriteLineAsync($"Order Id: {context.Message.OrderId}");
            await Console.Out.WriteLineAsync($"Order Item: {context.Message.OrderItem}");
        }
    }
}