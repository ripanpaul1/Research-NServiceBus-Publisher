
using System.Web.Services;
using System.Configuration;
using Lateetud.NServiceBus.Common;
using Lateetud.NServiceBus.Common.Models.NECGeneralAgent;
using Lateetud.NServiceBus.DAL;
using Lateetud.NServiceBus.DAL.NECGeneralAgent;
using System;

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
        MsmqSqlDBConfiguration msmqsqldbconfig = new MsmqSqlDBConfiguration(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString, 1, 5);

        //[WebMethod]
        //public string CreatePublisherQueues()
        //{
        //    var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("NEC.GeneralAgent.Publisher");
        //    msmqsqldbconfig.CreateEndpointInitializePipeline(endpointConfiguration).GetAwaiter().GetResult();
        //    return "Created publishers queues";
        //}


        [WebMethod]
        public string CreateServiceRequest(string message)
        {
            try
            {
                var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("NEC.GeneralAgent.Publisher");
                var id = "ga-" + Guid.NewGuid();
                msmqsqldbconfig.PublishedToBus(endpointConfiguration, new NECGeneralAgent { MessageID = id, Message = message });

                new NECGeneralAgentManager().Insert(id, message);

                return id;
            }
            catch(Exception err)
            {
                return "Server is down. Please try after sometime.";
            }
        }

        [WebMethod]
        public string CheckRequestStatus(string ID)
        {
            try
            {
                return ((GeneralAgentByGeneralAgentID_Select_Result) new NECGeneralAgentManager().Select(ID)).Status;
            }
            catch (Exception err)
            {
                return "Server is down. Please try after sometime.";
            }
        }

    }
}
