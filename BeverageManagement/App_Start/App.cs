using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeverageManagement.Models.EntityModel;
using BeverageManagement.Modules.Extensions;
namespace BeverageManagement.App_Start
{
    public static class App
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
                                LastEmployeeID = 0,
                                PerCyclePerson = 10
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
        private static string _senderMail;
        public static string SenderMail
        {
            get
            {
                return "rhasnatauto@gmail.com";
            }
        }

        private static string _senderPassword;
        public static string SenderPassword 
        { 
            get 
            {
                return "rashik1234";
            } 
        }
    }
}