using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Model.Command;

namespace OrderProducer
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

            await bus.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);

            try
            {
                await bus.Publish<ISubmitOrder>(new
                {
                    CommandId = InVar.Id,
                    ClientId = 5
                });
            }
            finally
            {
                await bus.StopAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
            }
        }
    }
}
