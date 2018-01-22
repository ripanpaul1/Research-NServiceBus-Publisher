using System.Web;
using System.Web.Mvc;

namespace Lateetud.NServiceBus.ServiceControl
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
