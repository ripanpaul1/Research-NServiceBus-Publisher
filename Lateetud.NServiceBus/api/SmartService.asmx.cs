using System;
using System.Web.Services;
using System.Configuration;
using Lateetud.NServiceBus.Common;
using Lateetud.NServiceBus.Common.Models.Smart;
using Lateetud.NServiceBus.DAL.Smart;
using Lateetud.NServiceBus.DAL.ef;

namespace Lateetud.NServiceBus.api
{
    /// <summary>
    /// Summary description for SmartService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SmartService : System.Web.Services.WebService
    {
        MsmqSqlDBConfiguration msmqsqldbconfig = new MsmqSqlDBConfiguration(ConfigurationManager.ConnectionStrings["Lateetud.db.conn"].ConnectionString, "smart.error", 1, 5);

        [WebMethod]
        public string CreatePublisherQueues()
        {
            try
            {
                var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("smart.publisher");
                msmqsqldbconfig.CreateEndpointInitializePipeline(endpointConfiguration).GetAwaiter().GetResult();

                return "Created publishers queues";
            }
            catch (Exception err)
            {
                return Messages.ServerDown;
            }
        }

        [WebMethod]
        public string InvokeOCRService(string processRef)
        {
            try
            {
                var requestId = "smo-" + Guid.NewGuid();
                var requestStatus = "InProgress";
                new SmartOcrManager().Insert(new SmartOcr { ServiceName = "InvokeOCRService", RequestId = requestId, Message = processRef, Status = requestStatus });
                msmqsqldbconfig.PublishedToBus(msmqsqldbconfig.ConfigureEndpoint("smart.publisher"), new OCR { RequestID = requestId, Message = processRef });
                return requestStatus;
            }
            catch (Exception err)
            {
                return Messages.ServerDown;
            }
        }

        [WebMethod]
        public string InvokeOCRServiceStatus(string requestId)
        {
            try
            {
                return ((SmartOcrByRequestId_Select_Result)new SmartOcrManager().Select(new SmartOcr { RequestId = requestId })).Status;
            }
            catch (Exception err)
            {
                return Messages.ServerDown;
            }
        }

        [WebMethod]
        public string InvokeRPAService(string rpaInput)
        {
            try
            {
                var requestId = "smr-" + Guid.NewGuid();
                var requestStatus = "InProgress";
                new SmartRpaManager().Insert(new SmartRpa { ServiceName = "InvokeRPAService", RequestId = requestId, Message = rpaInput, Status = requestStatus });
                msmqsqldbconfig.PublishedToBus(msmqsqldbconfig.ConfigureEndpoint("smart.publisher"), new RPA { RequestID = requestId, Message = rpaInput });
                return requestStatus;
            }
            catch (Exception err)
            {
                return Messages.ServerDown;
            }
        }

        [WebMethod]
        public string InvokeRPAServiceStatus(string requestId)
        {
            try
            {
                return ((SmartRpaByRequestId_Select_Result)new SmartRpaManager().Select(new SmartRpa { RequestId = requestId })).Status;
            }
            catch (Exception err)
            {
                return Messages.ServerDown;
            }
        }

    }
}
