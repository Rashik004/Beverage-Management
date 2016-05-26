using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeverageManagement.Models.EntityModel;

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
                using (var db = new BeverageManagementEntities())
                {
                    _config.CurrentRunningCycle = value.CurrentRunningCycle;
                    _config.LastEmployeeID = value.LastEmployeeID;
                    _config.PerCyclePerson = value.PerCyclePerson;
                    db.Entry(_config).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
    }
}