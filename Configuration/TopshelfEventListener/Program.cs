using System;
using Topshelf;

namespace TopshelfEventListener
{
    class Program
    {
        public static int Main()
        {
            return (int) HostFactory.Run(cfg =>
            {
                cfg.Service(x =>
                {
                    return new EventConsumerService();
                });
            });
        }
    }
}
