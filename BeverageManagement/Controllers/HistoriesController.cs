using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeverageManagement.Models.EntityModel;

namespace BeverageManagement.Controllers
{
    public class HistoriesController : Controller {
        private BeverageManagementEntities db = new BeverageManagementEntities();

        // GET: Histories
        public ActionResult Index() {
            return View(db.Employees.ToList());
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]

        public ActionResult InsertPastHistory(string employeeIds, DateTime dated) {
            Console.WriteLine(dated);
            var ids = employeeIds.Split(',');
            var history = new History();
            foreach (var id in ids) {
                history.EmployeeID = Int32.Parse(id);
                history.Dated = dated;
                db.Histories.Add(history);
            }
            try {
                db.SaveChanges();
            } catch {
                throw new Exception("We can't save the modified data.");
            }
            return RedirectToAction("Index");
        }


        public ActionResult SeePastHistoryByDate() {
            return View(db.Histories.ToList());
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
