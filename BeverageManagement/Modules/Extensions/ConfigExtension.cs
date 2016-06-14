using BeverageManagement.Models.EntityModel;
using DevMvcComponent.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeverageManagement.Modules.Extensions
{
    public static class ConfigExtension
    {
        public static void Save(this Config config)
        {
            using (var db = new BeverageManagementEntities())
            {
                db.Entry(config).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            var mailServer = new CustomMailServer(config.SiteName,config.ServerEmailSender, config.ServerEmailSenderPassword, config.ServerSmtpHost, config.ServerSmtpPort);
            DevMvcComponent.Mvc.Setup(config.SiteName, config.DevelopersEmails, System.Reflection.Assembly.GetExecutingAssembly(), mailServer);
            GC.Collect();
        }

    }
}