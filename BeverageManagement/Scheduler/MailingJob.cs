using DevMvcComponent;
using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using DevMvcComponent.Mail;
using DevMvcComponent.Enums;
namespace BeverageManagement.Scheduler
{
    public class MailingJob : IJob, IRegisteredObject
    {
        private readonly object _lock = new object();

        private bool _shuttingDown;

        public MailingJob()
        {
            // Register this job with the hosting environment.
            // Allows for a more graceful stop of the job, in the case of IIS shutting down.
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute()
        {
            lock (_lock)
            {
                if (_shuttingDown)
                    return;
                //var mailServer = new GmailServer("rhasnatauto@gmail.com", "rashik1234");
                //mailServer.QuickSend("rashikcse2k11@gmail.com", "mails will be sent", "we'll be sending mails to the employees selected for beverage payment this week.");
                Mvc.Mailer.QuickSend("rashikcse2k11@gmail.com", "hi!! mail sending reminder", "we'll be sending mails to the employees selected for beverage payment this week.", isAsync: false);
                // Do work, son!
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }
}