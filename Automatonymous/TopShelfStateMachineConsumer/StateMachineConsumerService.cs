using MassTransit;
using MassTransit.Saga;
using Model.State;
using Model.StateMachine;
using Topshelf;

namespace TopShelfStateMachineConsumer
{
    public class StateMachineConsumerService : ServiceControl
    {
        private IBusControl bus;
        private readonly OrderStateMachine machine;
        private readonly InMemorySagaRepository<OrderState> repository;

        public StateMachineConsumerService()
        {
            machine = new OrderStateMachine();
            repository = new InMemorySagaRepository<OrderState>();
        }

        public bool Start(HostControl hostControl)
        {
            bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("Order", e =>
                {
                    e.StateMachineSaga(machine, repository);
                });
            });

            bus.StartAsync();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            bus?.StopAsync();
            return true;
        }
    }
}