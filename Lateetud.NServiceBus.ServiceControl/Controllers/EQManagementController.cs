using Lateetud.NServiceBus.ServiceControl.Classes.MsmqReturnToSourceQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lateetud.NServiceBus.ServiceControl.Models;

namespace Lateetud.NServiceBus.ServiceControl.Controllers
{
    public class EQManagementController : Controller
    {
        
        // GET: EQManagement
        public ActionResult Index()
        {
            return View();
        }

        // GET: EQManagement
        public ActionResult ServiceControl()
        {
            var errorManager = new ErrorManager();
            errorManager.InputQueue = MsmqAddress.Parse("smart.error");
            return View(errorManager.ReturnAllMessages());
        }

        
    }

    
}