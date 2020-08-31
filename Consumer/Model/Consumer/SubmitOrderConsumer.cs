using System;
using System.Threading.Tasks;
using MassTransit;
using Model.Command;
using Model.Event;

namespace Model.Consumer
{
    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            Console.WriteLine("Command Received");
            await context.Publish<IOrderSubmitted>(new
            {
                EventId = context.Message.CommandId,
                ClientId = context.Message.ClientId
            });
            Console.WriteLine("Event Published");
        }
    }
}