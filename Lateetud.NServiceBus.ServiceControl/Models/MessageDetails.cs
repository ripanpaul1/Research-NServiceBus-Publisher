
namespace Lateetud.NServiceBus.ServiceControl.Models
{
    public class MessageDetails
    {
        public string QueueMessageId { get; set; }
        public string NServiceBusMessageId { get; set; }
        public string Message { get; set; }
    }
}