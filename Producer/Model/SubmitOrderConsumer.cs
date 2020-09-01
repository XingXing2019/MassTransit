using System;
using System.Threading.Tasks;
using MassTransit;

namespace Model
{
    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            await Console.Out.WriteLineAsync($"Command received. Command Id: {context.Message.CommandId}");
            await context.Publish<IOrderSubmitted>(new
            {
                context.Message.CommandId,
                context.Message.OrderId,
                context.Message.OrderDate,
                context.Message.OrderNumber,
                OrderItems = new[]
                {
                    new {context.Message.OrderItems[0].OrderId, context.Message.OrderItems[0].OrderNumber},
                    new {context.Message.OrderItems[1].OrderId, context.Message.OrderItems[1].OrderNumber},
                }
            });
        }
    }
}