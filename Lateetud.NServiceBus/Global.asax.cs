using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace Lateetud.NServiceBus
{
    public class Global : HttpApplication
    {
        public static IEndpointInstance EndpointInstance;
        public static IMessageSession MessageSession => EndpointInstance;

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitializeEndpoint().GetAwaiter().GetResult();
        }

        void Application_End(object sender, EventArgs e)
        {
            EndpointInstance.Stop().GetAwaiter().GetResult();
        }

        static async Task InitializeEndpoint()
        {
            var endpointConfiguration = new EndpointConfiguration("testqueue.Publisher");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            // Transport
            endpointConfiguration.UseTransport<MsmqTransport>();

            // Persistence
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var connection = @"Data Source=RIPANPC\SqlExpress;Initial Catalog=TestMsmqDB;UID=sa;Password=Password123;Integrated Security=True";
            persistence.SqlVariant(SqlVariant.MsSqlServer);
            persistence.ConnectionBuilder(connectionBuilder: () => new SqlConnection(connection));
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromSeconds(1));

            EndpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
        }
    }
}