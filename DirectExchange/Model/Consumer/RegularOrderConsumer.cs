using System;
using System.Threading.Tasks;
using MassTransit;
using Model.Event;

namespace Model.Consumer
{
    public class RegularOrderConsumer : IConsumer<IOrderSubmitted>
    {
        public async Task Consume(ConsumeContext<IOrderSubmitted> context)
        {
            await Console.Out.WriteLineAsync($"Regular order submitted. EventId: {context.Message.EventId}");
        }
    }
}