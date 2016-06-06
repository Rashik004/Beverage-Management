using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BeverageManagement.Constants;
using BeverageManagement.Models.EntityModel;
using Microsoft.AspNet.Identity;

namespace BeverageManagement.Controllers
{
    //[Authorize]// authentication
    [Authorize(Roles = RoleNames.Admin)]// authorization 

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
         
            Config config = AppConfig.Config;
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
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Config config)
        {
            if (ModelState.IsValid)
            {
                AppConfig.Config = config;
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
