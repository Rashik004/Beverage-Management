using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevMvcComponent.EntityConversion;
using System.Reflection;
using BeverageManagement.Modules.Extensions;
namespace BeverageManagement.Constants {
    public class RoleNames {
        public const string Admin = "Admin";
        public const string Editor = "EditorX";
        public const string Member = "MemberX";

        /// <summary>
        /// Returns all the roles name.
        /// </summary>
        /// <returns></returns>
        public static string[] GetRoles()
        {
            //var properties = RoleNames.
            var roles = typeof(RoleNames).GetConstants().Select(n => n.GetValue(null).ToString());
            return roles.ToArray();
        }

     

    }
}