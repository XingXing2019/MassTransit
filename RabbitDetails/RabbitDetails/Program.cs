using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using RabbitMQ.Client;
using Sample.Components;
using Sample.Contracts;

namespace Sample.Contracts
{
    public interface IUpdateAccount
    {
        string AccountNumber { get; }
    }
    public interface IDeleteAccount
    {
        string AccountNumber { get; }
    }
}

namespace Sample.Components
{
    public class AccountConsumer : IConsumer<IUpdateAccount>
    {
        public Task Consume(ConsumeContext<IUpdateAccount> context)
        {
            Console.WriteLine($"Command received: {context.Message.AccountNumber} on {context.ReceiveContext.InputAddress}");
            return Task.CompletedTask;
        }
    }

    public class AnotherAccountConsumer : IConsumer<IUpdateAccount>
    {
        public Task Consume(ConsumeContext<IUpdateAccount> context)
        {
            Console.WriteLine($"Another command received: {context.Message.AccountNumber}");
            return Task.CompletedTask;
        }
    }
}

namespace RabbitDetails
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                //// Replace Sample.Contracts:IUpdateAccount exchange with update-account exchange
                //cfg.Message<IUpdateAccount>(x => x.SetEntityName("update-account"));

                //cfg.ReceiveEndpoint("account-service", e =>
                //{
                //    //// Avoid binding account-service to Sample.Contracts:IUpdateAccount
                //    //e.ConfigureConsumeTopology = false;
                //    e.Lazy = true;
                //    e.PrefetchCount = 20;

                //    //// Bind account-service to account exchange
                //    //e.Bind("account");
                //    e.Consumer<AccountConsumer>();
                //});

                //cfg.ReceiveEndpoint("another-account-service", e =>
                //{
                //    e.PrefetchCount = 20;

                //    e.Consumer<AnotherAccountConsumer>();
                //});

                // Configure exchange for publisher
                cfg.Publish<IUpdateAccount>(x =>
                {
                    x.ExchangeType = ExchangeType.Direct;

                    // Move the msg to unmatched queue if the routingKey does not match
                    x.BindAlternateExchangeQueue("unmatched");
                });

                // Configure exchange for consumer
                cfg.ReceiveEndpoint("account-service-a", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Lazy = true;
                    e.PrefetchCount = 20;

                    e.Bind<IUpdateAccount>(e =>
                    {
                        e.ExchangeType = ExchangeType.Direct;
                        e.RoutingKey = "A";
                    });
                    e.Consumer<AccountConsumer>();
                });

                cfg.ReceiveEndpoint("account-service-b", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Lazy = true;
                    e.PrefetchCount = 20;

                    e.Bind<IUpdateAccount>(e =>
                    {
                        e.ExchangeType = ExchangeType.Direct;
                        e.RoutingKey = "B";
                    });
                    e.Consumer<AccountConsumer>();
                });
            });

            using var source = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await busControl.StartAsync(source.Token);

            try
            {
                Console.WriteLine("Bus was started");

                //// Send to specific exchange/queue, so only the consumer for this exchange/queue will receive the msg
                //var endpoint = await busControl.GetSendEndpoint(new Uri("exchange:account"));
                //await endpoint.Send<IUpdateAccount>(new
                //{
                //    AccountNumber = "12345"
                //});

                //// Send the msg to account exchange, but no consumer to for IDeleteAccount, so this msg will be moved to _skipped queue
                //await endpoint.Send<IDeleteAccount>(new
                //{
                //    AccountNumber = "67890"
                //});

                // Publish to Sample.Contracts:IUpdateAccount exchange, so both consumer will receive the msg
                await busControl.Publish<IUpdateAccount>(new
                {
                    AccountNumber = "12345"
                }, x => x.SetRoutingKey("A"));

                await busControl.Publish<IUpdateAccount>(new
                {
                    AccountNumber = "67890"
                }, x => x.SetRoutingKey("C"));

                await Task.Run(Console.ReadLine);
            }
            finally
            {
                await busControl.StopAsync(source.Token);
            }
        }
    }
}
