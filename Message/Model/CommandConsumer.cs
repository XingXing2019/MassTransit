using System;
using System.Threading.Tasks;
using MassTransit;

namespace Model
{
    public class CommandConsumer : IConsumer<IUpdateCustomerAddress>
    {
        public async Task Consume(ConsumeContext<IUpdateCustomerAddress> context)
        {
            await Console.Out.WriteLineAsync($"Command Id: {context.Message.CommandId}");
        }
    }
}