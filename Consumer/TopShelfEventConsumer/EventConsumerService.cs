using System;
using System.Threading;
using MassTransit;
using Model.Consumer;
using Model.Event;
using Topshelf;

namespace TopShelfEventConsumer
{
    public class EventConsumerService : ServiceControl
    {
        private IBusControl bus;
        private string queue = "order-submitted-service";
        public bool Start(HostControl hostControl)
        {
            bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(queue, e =>
                {
                    e.Consumer<OrderSubmittedConsumer>();

                    // Simple msg handler to consume msg on a receive endpoint
                    e.Handler<IOrderSubmitted>(async context =>
                    {
                        await Console.Out.WriteLineAsync($"Order submitted event received");
                    });
                });
            });

            bus.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token); 
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            bus.StopAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
            return true;
        }
    }
}