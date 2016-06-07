using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeverageManagement.Constants;
using BeverageManagement.Models;
using BeverageManagement.Modules.UserManagement;
using Microsoft.AspNet.Identity;

namespace BeverageManagement.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]// authorization
        public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult AddAsAdmin(string id) {
            Users.Manage.AddToRole(id, RoleNames.Admin);
            return View("Index", db.Users.ToList());
        }
        public ActionResult RemoveFromAdmin(string id) {
            if (Users.Manage.IsInRole(id, RoleNames.Admin))
                Users.Manage.RemoveFromRole(id, RoleNames.Admin);
            return View("Index", db.Users.ToList());
        }


        #region not necessary but kept for further assistance
        // GET: Admin/Details/5
        
        public ActionResult Details(string id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null) {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Admin/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser) {
            if (ModelState.IsValid) {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null) {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser) {
            if (ModelState.IsValid) {
                db.Entry(applicationUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(string id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null) {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id) {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
