using System;
using System.Threading.Tasks;
using MassTransit;
using Model.Event;

namespace Model.Consumer
{
    public class OrderSubmittedConsumer : IConsumer<IOrderSubmitted>
    {
        public async Task Consume(ConsumeContext<IOrderSubmitted> context)
        {
            Console.WriteLine("Event Received");
            await Console.Out.WriteLineAsync($"Client Id: {context.Message.ClientId} order submitted");
            await Console.Out.WriteLineAsync($"Event Id: {context.Message.EventId}");
        }
    }
}