using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeverageManagement.Models.EntityModel;
using BeverageManagement.Modules.Extensions;
namespace BeverageManagement
{
    public static class AppConfig
    {

        private static Config _config;
        
        public static Config Config
        {
            get
            {
                if (_config == null)
                {
                    using (var db = new BeverageManagementEntities())
                    {
                        _config = db.Configs.FirstOrDefault();
                       // db.Configs.SqlQuery("DELETE FROM Config");
                        if (_config == null)
                        {
                            _config = new Config()
                            {
                                CurrentRunningCycle = 1,
                                PerCyclePerson = 10,
                                AdminEmails = "admin1@site.com, admin2@site.com",
                                DefaultEmailBody = "Hello $name,<br /><br />You've been selected for payment of this cycle. Please pay the amount of tk $amount by this month.",
                                DefaultEmailFooter = "Thanks and regards,<br /><b>Administration</b><br />Site.com<br />Company<br />",
                                DevelopersEmails = "developer1@site.com,developer2@site.com",
                                EmailTemplateLocation = "EmailTemplate\\mail.cshtml",
                                ServerSmtpHost = "smtp.gmail.com",
                                ServerSmtpPort = 587,
                                ServerEmailSender = "senderemail@site.com",
                                ServerEmailSenderPassword = "default-password",
                                SiteName = "Site.com",
                                DefaultBeveragePrice = 100
                                
                            };
                            db.Configs.Add(_config);
                            db.SaveChanges();
                        }
                    }
                }
                return _config;
            }
            set
            {
                value.Save();
                _config = value;
            }
        }
        public static void SaveConfig()
        {
            _config.Save();
        }

        public static string Sitename
        {
            get { return AppConfig.Config.SiteName; }
        }
    }
}