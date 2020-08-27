using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Model;

namespace ConsoleEventListener
{
    class Program
    {
        static async Task Main()
        {
            var queue = "event_listener";
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint(queue, e =>
                {
                    e.Consumer<EventConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            

            // Declare exchange with name of namespace of message + message type
            // Declare queue with name of Endpoint
            // Bind queue to exchange
            await busControl.StartAsync(source.Token);

            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync(source.Token);
            }
        }
    }
}
