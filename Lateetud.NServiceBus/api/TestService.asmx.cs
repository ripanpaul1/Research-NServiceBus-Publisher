using System;
using System.Threading.Tasks;
using System.Web.Services;
using System.Configuration;
using NServiceBus;
using Lateetud.NServiceBus.Common;

namespace Lateetud.NServiceBus.api
{
    /// <summary>
    /// Summary description for TestService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TestService : System.Web.Services.WebService
    {
        MsmqSqlDBConfiguration msmqsqldbconfig = new MsmqSqlDBConfiguration(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString);

        [WebMethod]
        public string CreatePublisherQueues()
        {
            // if queue does not exists, created & got pipeline
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue1.publisher");
            msmqsqldbconfig.StartEndpoint(endpointConfiguration).GetAwaiter().GetResult();

            // if queue does not exists, created & got pipeline
            endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue2.publisher");
            msmqsqldbconfig.StartEndpoint(endpointConfiguration).GetAwaiter().GetResult();

            // if queue does not exists, created & got pipeline
            endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue3.publisher");
            msmqsqldbconfig.StartEndpoint(endpointConfiguration).GetAwaiter().GetResult();
        }

        [WebMethod]
        public string PublishToQueue1(string message)
        {
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue1.publisher");
            var testmessage = new TestMessage { Message = message };
            return msmqsqldbconfig.PublishedToBus(endpointConfiguration, testmessage);
        }

        [WebMethod]
        public string PublishToQueue2(string message)
        {
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue2.publisher");
            var testmessage = new TestMessage { Message = message };
            return msmqsqldbconfig.PublishedToBus(endpointConfiguration, testmessage);
        }

        [WebMethod]
        public string PublishToQueue3(string message)
        {
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue3.publisher");
            var testmessage = new TestMessage { Message = message };
            return msmqsqldbconfig.PublishedToBus(endpointConfiguration, testmessage);
        }

    }
}
