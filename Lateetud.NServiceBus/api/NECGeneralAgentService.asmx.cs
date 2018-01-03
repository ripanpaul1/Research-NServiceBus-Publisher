
using System;
using System.Messaging;
using System.Web.Services;
using System.Configuration;
using Lateetud.NServiceBus.Common;
using Lateetud.NServiceBus.Common.Models.NECGeneralAgent;
using Lateetud.NServiceBus.DAL;
using Lateetud.NServiceBus.DAL.NECGeneralAgent;
using Lateetud.NServiceBus.Classes.MsmqReturnToSourceQueue;

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

        [WebMethod]
        public string MessagesRestoreToSourceQueue()
        {

            var errorManager = new ErrorManager();
            errorManager.InputQueue = MsmqAddress.Parse("error");
            errorManager.ReturnAll();


            return "Message pick from error queue and restore to source queue";
        }

        [WebMethod]
        public string CreatePublisherQueues()
        {
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("NEC.GeneralAgent.Publisher");
            msmqsqldbconfig.CreateEndpointInitializePipeline(endpointConfiguration).GetAwaiter().GetResult();


            //if (!CreatePrivateQueues("error")) return "Unable to create queues";
            //if (!CreatePrivateQueues("NEC.GeneralAgent.Publisher", true)) return "Unable to create queues";
            //if (!CreatePrivateQueues("NEC.GeneralAgent.Subscriber", true)) return "Unable to create queues";



            return "Created publishers queues";
        }

        [WebMethod]
        public string DeleteQueues()
        {
            //if (!DeletePrivateQueue("error")) return "Unable to delete queue";
            //if (!DeletePrivateQueue("NEC.GeneralAgent.Publisher")) return "Unable to delete queue";
            //if (!DeletePrivateQueue("NEC.GeneralAgent.Subscriber")) return "Unable to delete queue";

            return "Queues are deleted";
        }


        [WebMethod]
        public string CreateServiceRequest(string message)
        {
            try
            {
                var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("NEC.GeneralAgent.Publisher");
                var id = "ga-" + Guid.NewGuid();
                msmqsqldbconfig.PublishedToBus(endpointConfiguration, new NECGeneralAgent { MessageID = id, Message = message });

                new NECGeneralAgentManager().Insert(id, null, message);

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


        #region CreatePrivateQueues

        #region CreatePrivateQueues [1]
        public bool CreatePrivateQueues(string QueueName)
        {
            return CreatePrivateQueues(QueueName, false);
        }
        #endregion

        #region CreatePrivateQueues [2]
        public bool CreatePrivateQueues(string QueueName, bool Transactional)
        {
            try
            {
                if (!MessageQueue.Exists(".\\Private$\\" + QueueName))
                {
                    MessageQueue.Create(".\\Private$\\" + QueueName, Transactional);
                }
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
        #endregion

        #endregion

        #region DeletePrivateQueue
        public bool DeletePrivateQueue(string QueueName)
        {
            try
            {
                if (MessageQueue.Exists(".\\Private$\\" + QueueName))
                {
                    MessageQueue.Delete(".\\Private$\\" + QueueName);
                }
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
        #endregion

    }
}
