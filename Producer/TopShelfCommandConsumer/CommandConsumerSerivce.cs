using System;
using System.Threading;
using MassTransit;
using Model;
using Topshelf;

namespace TopShelfCommandConsumer
{
    public class CommandConsumerSerivce : ServiceControl
    {
        private IBusControl bus;
        string queue = "submit-order-service";

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
                    e.Consumer<SubmitOrderConsumer>();
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