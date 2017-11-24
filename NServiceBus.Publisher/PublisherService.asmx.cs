using NServiceBus;
using System;
using System.Threading.Tasks;
using System.Web.Services;
using Messages;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace Publisher
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
                MessagePublishedToBus(message, "testqueue.Publisher").GetAwaiter().GetResult();
                return "Message published to bus";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // MessagePublishedToBus
        static async Task MessagePublishedToBus(string message, string EndpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            // Transport
            endpointConfiguration.UseTransport<MsmqTransport>();

            // Persistence
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlVariant(SqlVariant.MsSqlServer);
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString);
                });
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromDays(Convert.ToDouble(ConfigurationManager.AppSettings["Persistence.Subscriptions.CacheFor"])));

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            var rowmessage = new RowMessage
            {
                Message = message
            };
            await endpointInstance.Publish(rowmessage)
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

    }
}
