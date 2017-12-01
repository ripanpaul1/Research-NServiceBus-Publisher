
using System.Web.Services;
using System.Configuration;
using Lateetud.NServiceBus.Common;
using Lateetud.NServiceBus.Common.Models.NECGeneralAgent;

namespace Lateetud.NServiceBus.api
{
    /// <summary>
    /// Summary description for NECGeneralAgentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NECGeneralAgentService : System.Web.Services.WebService
    {
        MsmqSqlDBConfiguration msmqsqldbconfig = new MsmqSqlDBConfiguration(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString);

        [WebMethod]
        public string CreatePublisherQueues()
        {
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("NEC.GeneralAgent.Publisher");
            msmqsqldbconfig.CreateEndpointInitializePipeline(endpointConfiguration).GetAwaiter().GetResult();
            return "Created publishers queues";
        }

        [WebMethod]
        public string PublishNECGeneralAgent(string message)
        {
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("NEC.GeneralAgent.Publisher");
            return msmqsqldbconfig.PublishedToBus(endpointConfiguration, new NECGeneralAgent { Message = message });
        }

        [WebMethod]
        public string PublishNECGeneralAgentResult(string message)
        {
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("NEC.GeneralAgent.Publisher");
            return msmqsqldbconfig.PublishedToBus(endpointConfiguration, new NECGeneralAgentResult { Message = message });
        }

    }
}
