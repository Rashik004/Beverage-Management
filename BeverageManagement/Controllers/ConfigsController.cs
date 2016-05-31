using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeverageManagement.Models.EntityModel;

namespace BeverageManagement.Controllers
{
    public class ConfigsController : Controller
    {
        private BeverageManagementEntities db = new BeverageManagementEntities();

        // GET: Configs
        public ActionResult Index()
        {
            return View(db.Configs.ToList());
        }

        // GET: Configs/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Config config = db.Configs.Find(id);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }

      

        // GET: Configs/Edit/5
        public ActionResult Edit()
        {
         
            Config config = App.Config;
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }

        // POST: Configs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConfigID,PerCyclePerson,LastEmployeeID,CurrentRunningCycle")] Config config)
        {
            if (ModelState.IsValid)
            {
                App.Config = config;
                return RedirectToAction("Index");
            }
            return View(config);
        }

       

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
