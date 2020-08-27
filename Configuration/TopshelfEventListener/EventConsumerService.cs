using System;
using MassTransit;
using Model;
using Topshelf;

namespace TopshelfEventListener
{
    public class EventConsumerService : ServiceControl
    {
        private IBusControl _bus;
        private string queue = "event_listener";
        public bool Start(HostControl hostControl)
        {
            _bus = ConfigureBus();
            _bus.StartAsync(TimeSpan.FromSeconds(10));
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _bus?.StopAsync(TimeSpan.FromSeconds(10));
            return true;
        }

        private IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint(queue, e =>
                {
                    e.Consumer<EventConsumer>();
                });
            });
        }
    }
}