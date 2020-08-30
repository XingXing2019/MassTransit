using System;
using System.Threading;
using MassTransit;
using Model;
using Topshelf;

namespace TopShelfCommandListener
{
    public class CommandConsumerService : ServiceControl
    {
        private IBusControl bus;
        private string queue = "update-address-service";
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
                    e.Consumer<CommandConsumer>();
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