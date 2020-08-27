using System;
using System.Threading;
using MassTransit;
using System.Threading.Tasks;
using Model;


namespace ConsoleEventPublisher
{
    class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq();

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);

            try
            {
                do
                {
                    string value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)"); 
                        Console.Write(">");
                        return Console.ReadLine();
                    });
                    if("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    // Declare exchange with name of namespace of message + message type
                    await busControl.Publish<ValueEntered>(new
                    {
                        Value = value
                    });
                } 
                while (true);
            }
            finally
            {
                await busControl.StopAsync(source.Token);
            }
        }
    }
}
