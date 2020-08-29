using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Model;

namespace RabbitMqConsoleProducer
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

            var source = new CancellationTokenSource();

            await bus.StartAsync(source.Token);

            try
            {
                do
                {
                    string orderItem = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter Order Item (or quit to exit)");
                        Console.Write(">");
                        return Console.ReadLine();
                    });
                    if (orderItem.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
                        break;

                    // Declare exchange Model.IOrderSubmitted and publish msg to this exchange
                    await bus.Publish<IOrderSubmitted>(new
                    {
                        OrderId = Guid.NewGuid(),
                        OrderItem = orderItem
                    });
                } 
                while (true);
            }
            finally
            {
                await bus.StopAsync(source.Token);
            }
        }
    }
}
