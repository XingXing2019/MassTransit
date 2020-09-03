using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Model;
using Model.Event;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EventProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var exchange = "Model.Event:IOrderSubmitted";
            var priorityRoutingKey = RoutingKey.Priority;
            var regularRoutingKey = RoutingKey.Regular;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var priorityOrderSubmitted = new OrderSubmitted
                {
                    CorrelationId = InVar.CorrelationId,
                    EventId = Guid.NewGuid(),
                    OrderType = priorityRoutingKey
                };

                var regularOrderSubmitted = new OrderSubmitted
                {
                    CorrelationId = InVar.CorrelationId,
                    EventId = Guid.NewGuid(),
                    OrderType = regularRoutingKey
                };


                // Msg should be wrapped in an envelop before sending to RabbitMQ
                var priorityEnvelope = new Envelope
                {
                    MessageId = Guid.NewGuid().ToString(),
                    DestinationAddress = "localhost",
                    Message = priorityOrderSubmitted,
                    Headers = { },
                    MessageType = new[] {$"urn:message:{exchange}"}
                };
                
                var regularEnvelope = new Envelope
                {
                    MessageId = Guid.NewGuid().ToString(),
                    DestinationAddress = "localhost",
                    Message = priorityOrderSubmitted,
                    Headers = { },
                    MessageType = new[] { $"urn:message:{exchange}" }
                };

                var priorityMsg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(priorityEnvelope));
                var regularMsg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(regularEnvelope));

                channel.BasicPublish(exchange, priorityRoutingKey, null, priorityMsg);
                channel.BasicPublish(exchange, regularRoutingKey, null, regularMsg);
            }
        }
    }
}
