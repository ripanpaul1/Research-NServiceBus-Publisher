using Lateetud.NServiceBus.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;

namespace Lateetud.NServiceBus
{
    public class Global : HttpApplication
    {
        
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            MsmqSqlDBConfiguration msmqsqldbconfig = new MsmqSqlDBConfiguration(ConfigurationManager.ConnectionStrings["SqlPersistence"].ConnectionString);


            // if queue does not exists, created & got pipeline
            var endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue1");
            msmqsqldbconfig.StartEndpoint(endpointConfiguration).GetAwaiter().GetResult();

            // if queue does not exists, created & got pipeline
            endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue2");
            msmqsqldbconfig.StartEndpoint(endpointConfiguration).GetAwaiter().GetResult();

            // if queue does not exists, created & got pipeline
            endpointConfiguration = msmqsqldbconfig.ConfigureEndpoint("queue3");
            msmqsqldbconfig.StartEndpoint(endpointConfiguration).GetAwaiter().GetResult();
            

        }
    }
}