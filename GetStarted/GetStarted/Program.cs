using System;
using System.Threading.Tasks;
using MassTransit;

namespace GetStarted
{
    public class Message
    {
        public string Text { get; set; }
    }
    class Program
    {
        public static async Task Main()
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("127.0.0.1");
                sbc.ReceiveEndpoint("test_queue", ep =>
                {
                    ep.Handler<Message>(context => Console.Out.WriteAsync($"Received: {context.Message.Text}"));
                });
            });

            // It is important to start the bus
            await bus.StartAsync();

            await bus.Publish(new Message {Text = "Hello"});

            Console.WriteLine("Press any key to exit");

            await Task.Run(() => Console.ReadKey());

            await bus.StopAsync();
        }
    }
}
