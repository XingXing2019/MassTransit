using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Model;

namespace RabbitMqConsoleListener
{
    class Program
    {
        public static async Task Main()
        {
            var queue = "order-event-listener";
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(queue, e =>
                {
                    e.Consumer<OrderSubmittedConsumer>();
                });
            });

            var source = new CancellationTokenSource();

            // Declare Declare exchanges Model.IOrderSubmitted and order-event-listener
            // Bind order-event-listener to Model.IOrderSubmitted
            // Declare queue order-event-listener
            // Bind queue order-event-listener to exchange order-event-listener
            await bus.StartAsync(source.Token);

            try
            {
                Console.WriteLine("Press enter to exit");
                await Task.Run(() => { return Console.ReadLine(); });
            }
            finally
            {
                await bus.StopAsync(source.Token);
            }
        }
    }
}
