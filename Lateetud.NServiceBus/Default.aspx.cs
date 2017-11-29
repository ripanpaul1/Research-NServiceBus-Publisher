using Lateetud.NServiceBus.Common;
using NServiceBus;
using System;
using System.Threading.Tasks;
using System.Web.UI;

namespace Lateetud.NServiceBus
{
    public partial class _Default : Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                StartEndpoint.InitializeEndpoint("testqueue.Publisher").GetAwaiter().GetResult();
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            MessagePublishedToBue(txt.Text).GetAwaiter().GetResult();
        }

        public async Task MessagePublishedToBue(string message)
        {
            object rowmessage = new TestMessage
            {
                Message = message
            };

            await StartEndpoint.endpointInstance.Publish(rowmessage, new PublishOptions())
                .ConfigureAwait(false);
        }


    }
}