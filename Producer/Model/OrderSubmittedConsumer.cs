using System;
using System.Threading.Tasks;
using MassTransit;

namespace Model
{
    public class OrderSubmittedConsumer : IConsumer<IOrderSubmitted>
    {
        public async Task Consume(ConsumeContext<IOrderSubmitted> context)
        {
            await Console.Out.WriteLineAsync($"Command Id: {context.Message.CommandId}");
            await Console.Out.WriteLineAsync($"Order Id: {context.Message.OrderId}");
            await Console.Out.WriteLineAsync($"Order Date: {context.Message.OrderDate}");
            await Console.Out.WriteLineAsync($"Order Number: {context.Message.OrderNumber}");
            var orderItems = context.Message.OrderItems;
            foreach (var orderItem in orderItems)
            {
                await Console.Out.WriteLineAsync($"Order Item Number: {orderItem.OrderNumber}");
            }
        }
    }
}