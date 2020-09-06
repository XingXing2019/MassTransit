using System;
using System.Reflection.Metadata.Ecma335;
using Automatonymous;
using Model.Command;
using Model.Event;
using Model.State;

namespace Model.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        // Declares two states
        public Automatonymous.State Submitted { get; set; }
        public Automatonymous.State Accepted { get; set; }

        // Declares event using Event<T>, T must be a valid msg type
        public Event<SubmitOrder> SubmitOrder { get; set; }
        public Event<OrderAccepted> OrderAccepted { get; set; }
        public Event<OrderCanceled> OrderCanceled { get; set; }
        public Event<OrderReady> OrderReady { get; set; }


        public OrderStateMachine()
        {
            // Declares property to hold the instance states: 0 - None, 1 - Initial, 2 - Final, 3 - Submitted, 4 - Accepted
            InstanceState(x =>x.CurrentState, Submitted, Accepted);

            // Adds composite event to state machine, the composite event will be triggered when all the required events has been raised
            CompositeEvent(() => OrderReady, x => x.ReadyEventStatus, SubmitOrder, OrderAccepted);

            // To increase new instance performance, configuring an event to directly insert into a saga repository
            // To configure an event to insert, it should be in the Initially block, as well as have a saga factory specified
            Event(() => SubmitOrder, e =>
            {
                e.CorrelateById(context => context.Message.OrderId);
                e.InsertOnInitial = true;
                e.SetSagaFactory(context => new OrderState
                {
                    CorrelationId = context.Message.OrderId
                });
            });
            Event(() => OrderAccepted, e =>
            {
                e.CorrelateById(context => context.Message.OrderId);
                e.InsertOnInitial = true;
                e.SetSagaFactory(context => new OrderState
                {
                    CorrelationId = context.Message.OrderId
                });
            });

            // Declares event on state machine with specific date type, and allows the correlation of event to be configured
            Event(() => OrderCanceled, e => e.CorrelateById(context => context.Message.OrderId));

            // Configures the behave of the state machine when specific event was received
            Initially(
                When(SubmitOrder)
                    .TransitionTo(Submitted), 
                When(OrderAccepted)
                    .TransitionTo(Accepted));

            During(Submitted, 
                When(OrderAccepted)
                    .TransitionTo(Accepted));

            During(Accepted, 
                When(SubmitOrder)
                    .Then(x =>x.Instance.OrderDate = x.Data.OrderDate));

            DuringAny(
                When(OrderReady)
                    .Then(context => Console.WriteLine($"Order Ready: {context.Instance.CorrelationId}")));
        }
    }
}