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
            var config = App.Config;
            var mailServer = new CustomMailServer(config.ServerEmailSender, config.ServerEmailSenderPassword, config.ServerSmtpHost, config.ServerSmtpPort);
            DevMvcComponent.Mvc.Setup(config.SiteName, config.DevelopersEmails, System.Reflection.Assembly.GetExecutingAssembly(), mailServer);
            JobManager.Initialize(new MailScheduler());
        }
    }
}
