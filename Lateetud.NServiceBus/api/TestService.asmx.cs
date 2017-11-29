using System;
using System.Threading.Tasks;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using Lateetud.NServiceBus.Common;
using System.Collections.Generic;

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
        [WebMethod]
        public string MyPublisher(string message)
        {
            MsmqSqlDBConfiguration msmqsqldbconfig = new MsmqSqlDBConfiguration(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString);
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("testqueue.Publisher");
            var testmessage = new TestMessage { Message = message };
            return msmqsqldbconfig.PublishedToBus(endpointConfiguration, testmessage);
        }

        [WebMethod]
        public string MySubscriber()
        {
            MsmqSqlDBConfiguration msmqsqldbconfig = new MsmqSqlDBConfiguration(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString);
            List<PublisherEndpoints> publisherEndpoints = new List<PublisherEndpoints>();
            publisherEndpoints.Add(new PublisherEndpoints(endpointName: "testqueue.Publisher", messageType: typeof(TestMessage)));
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("testqueue.Subscriber", publisherEndpoints);
            msmqsqldbconfig.StartEndpoint(endpointConfiguration).GetAwaiter().GetResult();
            return "Endpoint: testqueue.Subscriber is started";
        }
    }
}
