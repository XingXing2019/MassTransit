using System;
using Topshelf;

namespace TopShelfStateMachineConsumer
{
    class Program
    {
        public static int Main()
        {
            return (int) HostFactory.Run(cfg =>
            {
                cfg.Service(x => new StateMachineConsumerService());
            });
        }
    }
}
