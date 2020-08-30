using System;
using System.Threading.Tasks;
using MassTransit;

namespace Model
{
    public class EventConsumer : IConsumer<ICustomerAddressUpdated>
    {
        public async Task Consume(ConsumeContext<ICustomerAddressUpdated> context)
        {
            await Console.Out.WriteLineAsync($"Event Id: {context.Message.EventId}");
        }
    }
}