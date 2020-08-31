using System;
using Topshelf;

namespace TopShelfCommandConsumer
{
    class Program
    {
        public static int Main()
        {
            return (int) HostFactory.Run(cfg =>
            {
                cfg.Service(x => new CommandConsumerService());
            });
        }
    }
}
