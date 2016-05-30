using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeverageManagement.Models.EntityModel;
using BeverageManagement.App_Start;
using DevMvcComponent.Pagination;
using DevMvcComponent.Mail;
using System.Threading.Tasks;

namespace BeverageManagement.Controllers
{
    public class PaymentCyclesController : Controller
    {
        private BeverageManagementEntities db = new BeverageManagementEntities();
       // private PaginationInfo pageInfo;

        // GET: PaymentCycles
        //private db.employee asd;
        //private object pagedEmployees;
       // private PaginationInfo pageInfo;
 
        private PaginationInfo GetPageInfo(int perCyclePerson, int pageNumber)
        {
            var pageInfo = new PaginationInfo
            {
                ItemsInPage = perCyclePerson,
                PageNumber = pageNumber,

            };
            return pageInfo;
        }

        //private int SelectEmployeeForPayment(int expectedNumberOfEmployee, int currentRunningCycle)
        //{
        //    var employees = db.Employees;
        //    var pageInfo = GetPageInfo(expectedNumberOfEmployee, 1);
        //    var pagedEmployees = employees
        //                .Where(e => e.Cycle < currentRunningCycle && e.IsWorking == true)
        //                .OrderBy(n => n.EmployeeID)
        //                .GetPageData(pageInfo).ToList();
        //    Console.WriteLine(pagedEmployees.GetType());
        //    var deb = pagedEmployees.GetType();
        //    foreach (var employee in pagedEmployees)
        //    {

        //        employee.Cycle++;
        //        try
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
        //                History history = new History();
        //                history.EmployeeID = employee.EmployeeID;
        //                history.Dated = DateTime.Now;
        //                db.Histories.Add(history);
        //            }
        //        }
        //        catch
        //        {
        //            throw new Exception("We can't save the modified data.");
        //        }
        //    }
        //    db.SaveChanges();
        //    return pagedEmployees.Count();
        //}

        private List<Employee> SelectEmployeesForPayment(int expectedNumberOfEmployee, int currentRunningCycle)
        {
            var employees = db.Employees;
            var pageInfo = GetPageInfo(expectedNumberOfEmployee, 1);
            var pagedEmployees = employees
                        .Where(e => e.Cycle < currentRunningCycle && e.IsWorking == true)
                        .OrderBy(n => n.EmployeeID)
                        .GetPageData(pageInfo).ToList();


            return pagedEmployees;
        }

        public ActionResult ProcessPayment()
        {
            var config = App.Config;
            
            var selectedEmployeesForPayment = SelectEmployeesForPayment(config.PerCyclePerson, config.CurrentRunningCycle);
            int debugging = selectedEmployeesForPayment.Count();
            if (selectedEmployeesForPayment.Count()< config.PerCyclePerson)
            {
                config.CurrentRunningCycle++;
                App.Config = config;
                var NewlySelectedEmployeesForPayment= SelectEmployeesForPayment(config.PerCyclePerson-selectedEmployeesForPayment.Count(), config.CurrentRunningCycle);
                selectedEmployeesForPayment = selectedEmployeesForPayment.Concat(NewlySelectedEmployeesForPayment).ToList();
            }

            return View(selectedEmployeesForPayment);
            //Payment(selectedEmployeesForPayment, "Random", "hello!!!you've been selected!!", true);
            //return View("Index");
        }
        //private void Payment(List<Employee> selectedEmployeesForPayment, string emailSubject, string emailBody, bool isConfirmed=true)
        //{
        //    if (isConfirmed == false)
        //    {
        //       // return View("Index");
        //        return;
        //    }
        //    else
        //    {
        //        var mailServer = new GmailServer(App.SenderMail, App.SenderPassword);

        //        foreach (var employee in selectedEmployeesForPayment)
        //        {

        //            employee.Cycle++;
        //            try
        //            {
        //                if (ModelState.IsValid)
        //                {
        //                    db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
        //                    History history = new History();
        //                    history.EmployeeID = employee.EmployeeID;
        //                    history.Dated = DateTime.Now;
        //                    mailServer.QuickSend(employee.Email, emailSubject, emailBody);
        //                    db.Histories.Add(history);

        //                }                        
        //            }
        //            catch
        //            {
        //                throw new Exception("We can't save the modified data.");
        //            }
                    
        //        }
        //        db.SaveChanges();
        //       // return View("Index");
        //    }

        //}
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ProcessPayment( bool isConfirmed=true)
        {

            var config = App.Config;
            var emailSubject = Request.Form["emailSubject"];
            var emailBody = Request.Form["emailBody"];

            var selectedEmployeesForPayment = SelectEmployeesForPayment(config.PerCyclePerson, config.CurrentRunningCycle);
            int debugging = selectedEmployeesForPayment.Count();
            if (selectedEmployeesForPayment.Count() < config.PerCyclePerson)
            {
                config.CurrentRunningCycle++;
                App.Config = config;
                var NewlySelectedEmployeesForPayment = SelectEmployeesForPayment(config.PerCyclePerson - selectedEmployeesForPayment.Count(), config.CurrentRunningCycle);
                selectedEmployeesForPayment = selectedEmployeesForPayment.Concat(NewlySelectedEmployeesForPayment).ToList();
            }
            if(isConfirmed==false)
            {
                return View("Index");
            }
            else
            {
                var mailServer = new GmailServer(App.SenderMail, App.SenderPassword);

                foreach (var employee in selectedEmployeesForPayment)
                {
                     
                    employee.Cycle++;
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                            History history = new History();
                            history.EmployeeID = employee.EmployeeID;
                            history.Dated = DateTime.Now;
                            db.Histories.Add(history);
                           // mailServer.QuickSend(employee.Email, emailSubject, emailBody);
                        }
                    }
                    catch
                    {
                        throw new Exception("We can't save the modified data.");
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }
        public ActionResult Index(int page = 1)
        {

            var config = App.Config;
            var employees = db.Employees;
            var histories = db.Histories;
            var pageInfo = GetPageInfo(config.PerCyclePerson, page);
            var employeePaymentHistory = histories
                                    .Include(n => n.Employee)
                                    .OrderBy(e => e.HistoryID)
                                    .GetPageData(pageInfo).ToList();
            
            ViewBag.paginationHtml = new MvcHtmlString(Pagination.GetList(pageInfo, url: "?page=@page"));

            return View(employeePaymentHistory);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: PaymentCycles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentCycles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,Name,IsWorking,Cycle,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: PaymentCycles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: PaymentCycles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,Name,IsWorking,Cycle,Email")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: PaymentCycles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: PaymentCycles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
