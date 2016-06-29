using System;
using System.Linq;
using System.Web.Mvc;
using BeverageManagement.BusinessLogic;
using BeverageManagement.Models.EntityModel;

namespace BeverageManagement.Controllers {
    [Authorize] // authentication or login : must login
    public class HistoriesController : Controller {
        private BeverageManagementEntities db = new BeverageManagementEntities();
        private Logic _logic;

        public HistoriesController() {
            _logic = new Logic(db);
        }

        // GET: Histories
        public ActionResult Index() {
            db.Employees.FirstOrDefault();
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


        public ActionResult ByDate(DateTime? dated) {
            var datesList = _logic.GetHistoryDates();
            if (!dated.HasValue) {
                var historyByDateViewModel = datesList.FirstOrDefault();
                if (historyByDateViewModel != null) {
                    dated = historyByDateViewModel.Date;
                }
            }
            var employeesHistory = _logic.GetEmployeesHistoryByDate(dated.Value);
            ViewBag.datesList = datesList;
            ViewBag.dated = dated;
            return View(employeesHistory);
        }
        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
