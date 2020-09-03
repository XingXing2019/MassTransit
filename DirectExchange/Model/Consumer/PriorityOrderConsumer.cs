using System;
using System.Threading.Tasks;
using MassTransit;
using Model.Event;

namespace Model.Consumer
{
    public class PriorityOrderConsumer : IConsumer<IOrderSubmitted>
    {
        public async Task Consume(ConsumeContext<IOrderSubmitted> context)
        {
            await Console.Out.WriteLineAsync($"Priority order submitted. EventId: {context.Message.EventId}");
        }
    }
}