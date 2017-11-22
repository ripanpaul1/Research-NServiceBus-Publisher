using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Publisher
{
    public class StartEndpoint
    {
        public static IEndpointInstance endpointInstance = null;

        //public StartEndpoint(string EndpointName)
        //{
            
        //    InitializeEndpoint(EndpointName).GetAwaiter().GetResult();
        //}

        public async static Task InitializeEndpoint(string EndpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            
        }

    }
}