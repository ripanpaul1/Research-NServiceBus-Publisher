
using System;
using System.Threading.Tasks;
using System.Web.Services;
using NServiceBus;
using Lateetud.NServiceBus.Common;
using NServiceBus.Persistence.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace Lateetud.NServiceBus
{
    /// <summary>
    /// Summary description for PublisherService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PublisherService : System.Web.Services.WebService
    {

        [WebMethod]
        public string MyService(string message)
        {
            try
            {
                Global.MessageSession.Publish(new TestMessage
                {
                    Message = message
                }).GetAwaiter().GetResult();
                return "Message published to bus";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        

    }
}
