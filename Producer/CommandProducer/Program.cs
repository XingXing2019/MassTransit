using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport.Topology.Entities;
using Model;

namespace CommandProducer
{
    class Program
    {
        public static async Task Main()
        {
            var queue = "submit-order-service";
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
                var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{queue}"));
                await endpoint.Send<ISubmitOrder>(new
                {
                    CommandId = InVar.Id,
                    OrderId = Guid.NewGuid(),
                    OrderDate = DateTime.Now,
                    OrderNumber = "123",
                    OrderItems = new[]
                    {
                        new {OrderId = Guid.NewGuid(), OrderNumber = "abc"},
                        new {OrderId = Guid.NewGuid(), OrderNumber = "efg"},
                    }
                });
            }
            finally
            {
                await bus.StopAsync(source.Token);
            }
        }
    }
}
