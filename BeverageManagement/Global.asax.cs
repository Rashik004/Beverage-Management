using DevMvcComponent.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DevMvcComponent;
using FluentScheduler;
using BeverageManagement.BusinessLogic;
namespace BeverageManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            JobManager.Initialize(new MailScheduler()); 
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Mvc.Mailer.QuickSend();
            var mailServer = new GmailServer("rhasnatauto@gmail.com", "rashik1234");
            DevMvcComponent.Mvc.Setup("Beverage Management","rhasnat93@gmail.com",System.Reflection.Assembly.GetExecutingAssembly(), mailServer);
            JobManager.Initialize(new MailScheduler()); 
        }
    }
}
