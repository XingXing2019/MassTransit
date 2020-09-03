using System.Threading.Tasks;
using MassTransit;
using Model;
using Model.Consumer;
using Model.Event;
using Topshelf;
using RabbitMQ.Client;

namespace TopShelfEventConsumer
{
    public class EventConsumerService : ServiceControl
    {
        private IBusControl bus;
        private string priorityRoutingKey = RoutingKey.Priority;
        private string regularRoutingKey = RoutingKey.Regular;

        private string priorityQueue = "PriorityQueue";
        private string regularQueue = "RegularQueue";
        public bool Start(HostControl hostControl)
        {
            bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, priorityQueue, endpoint =>
                {
                    endpoint.BindMessageExchanges = false;

                    endpoint.Bind<IOrderSubmitted>(x =>
                    {
                        x.RoutingKey = priorityRoutingKey;
                        x.ExchangeType = ExchangeType.Direct;
                    });

                    endpoint.Consumer<PriorityOrderConsumer>();
                });

                cfg.ReceiveEndpoint(host, regularQueue, endpoint =>
                {
                    endpoint.BindMessageExchanges = false;

                    endpoint.Bind<IOrderSubmitted>(x =>
                    {
                        x.RoutingKey = regularRoutingKey;
                        x.ExchangeType = ExchangeType.Direct;
                    });

                    endpoint.Consumer<RegularOrderConsumer>();
                });
            });

            bus.StartAsync();
            return true;
        }

        public  bool Stop(HostControl hostControl)
        {
            bus?.StopAsync();
            return true;
        }
    }
}