using System.IO;
using System.Threading.Tasks;
using MassTransit;
using Model.Command;
using Model.Event;

namespace Model.Consumer
{
    public class LogOrderSubmittedConsumer : IConsumer<ISubmitOrder>
    {
        private readonly TextWriter _writer;

        public LogOrderSubmittedConsumer(TextWriter writer)
        {
            _writer = writer;
        }

        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            await _writer.WriteLineAsync($"Client Id: {context.Message.ClientId} order submitted");
            await _writer.WriteLineAsync($"Command Id: {context.Message.CommandId}");
        }
    }
}