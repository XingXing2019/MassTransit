using System.Collections.Generic;

namespace Model
{
    public class Envelope
    {
        public string DestinationAddress { get; set; }
        public string MessageId { get; set; }
        public string ConversationId { get; set; }
        public string SourceAddress { get; set; }
        public IDictionary<string, object> Headers { get; set; }
        public object Message { get; set; }
        public string[] MessageType { get; set; }
    }
}