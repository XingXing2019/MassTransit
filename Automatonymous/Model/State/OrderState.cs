using System;
using Automatonymous;

namespace Model.State
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
        public int ReadyEventStatus { get; set; }
        
        public DateTime? OrderDate { get; set; }
    }
}