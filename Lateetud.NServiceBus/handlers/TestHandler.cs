using Lateetud.NServiceBus.Common;
using NServiceBus;
using System.Threading.Tasks;

namespace Lateetud.NServiceBus.handlers
{
    public class TestHandler: IHandleMessages<TestMessage>
    {
        public Task Handle(TestMessage message, IMessageHandlerContext context)
        {
            return Task.CompletedTask;
        }
    }
}