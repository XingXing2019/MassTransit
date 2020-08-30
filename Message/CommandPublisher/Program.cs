using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Model;

namespace CommandPublisher
{
    class Program
    {
        public static async Task Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await bus.StartAsync(source.Token);

            try
            {
                var endpoint = await bus.GetSendEndpoint(new Uri("queue:update-address-service"));

                // Set CorrelationId using SendContext<T>
                await endpoint.Send<IUpdateCustomerAddress>(new { CommandId = InVar.Id },
                    context => context.CorrelationId = context.Message.CommandId);
            }
            finally
            {
                await bus.StopAsync(source.Token);
            }
        }
    }
}
