using System;
using System.Threading;
using MassTransit;
using Model;
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
                });
            });

            bus.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            bus?.StopAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
            return true;
        }
    }
}