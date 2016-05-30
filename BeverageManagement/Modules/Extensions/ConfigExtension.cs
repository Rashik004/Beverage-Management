using BeverageManagement.Models.EntityModel;
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
        }

    }
}