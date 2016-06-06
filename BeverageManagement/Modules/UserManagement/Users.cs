using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeverageManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace BeverageManagement.Modules.UserManagement {
    public static class Users {

        private static ApplicationUserManager _userManager;

        public static ApplicationUserManager Manage {
            get { return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }


        private static RoleManager<IdentityRole> _roleManager;

        public static RoleManager<IdentityRole> Roles {
            get {
                if (_roleManager == null) {
                    var db = new ApplicationDbContext();
                    _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                }
                return _roleManager;
            }
            private set { _roleManager = value; }
        }
    }
}