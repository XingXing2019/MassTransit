using System;
using System.Threading.Tasks;
using MassTransit;

namespace Model
{
    public class EventConsumer : IConsumer<ValueEntered>
    {
        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            await Console.Out.WriteLineAsync($"Value: {context.Message.Value}");
        }
    }
}