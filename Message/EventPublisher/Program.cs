using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Model;

namespace EventPublisher
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
                await bus.Publish<ICustomerAddressUpdated>(new {EventId = InVar.Id},
                    context => context.CorrelationId = context.Message.EventId);
            }
            finally
            {
                await bus.StopAsync(source.Token);
            }
        }
    }
}
