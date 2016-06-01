﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeverageManagement.Models.EntityModel;
using DevMvcComponent.Pagination;
using DevMvcComponent.Mail;
using System.Threading.Tasks;
using BeverageManagement.BusinessLogic;
using BeverageManagement.ViewModel;
using DevMvcComponent;
namespace BeverageManagement.Controllers
{
    public class PaymentCyclesController : Controller
    {
        #region Attributes
        private BeverageManagementEntities db = new BeverageManagementEntities();
        private Logic _logic;
        #endregion

        #region Constructor
        public PaymentCyclesController()
        {
            _logic = new Logic(db);
        }
        #endregion

        #region Process Payment (GET)
        public ActionResult ProcessPayment()
        {
            var config = AppConfig.Config;
            bool isCycleChanged;
            var employees = _logic.GetFinalSelectedEmployeesForCycle(config.PerCyclePerson, config.CurrentRunningCycle, out isCycleChanged);
            if (isCycleChanged)
            {
                config.CurrentRunningCycle++;
                AppConfig.SaveConfig();
            }
            TempData["SelectedEmployees"] = employees;
            return View(employees);
        }
        #endregion

        #region Process Payment (POST)
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> ProcessPayment(EmailDetails emailInfo)
        {

            var config = AppConfig.Config;
            var emailSubject = emailInfo.emailSubject;
            var emailBody = emailInfo.emailBody;
            emailBody = emailBody.Replace("$amount", AppConfig.Config.DefaultBeveragePrice.ToString());
            var selectedEmployeesForPayment = (List<Employee>)TempData["SelectedEmployees"];

            foreach (var employee in selectedEmployeesForPayment)
            {
                employee.Cycle = AppConfig.Config.CurrentRunningCycle;
                if (ModelState.IsValid)
                {
                    db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                    History history = new History();
                    history.EmployeeID = employee.EmployeeID;
                    history.Dated = DateTime.Now;
                    db.Histories.Add(history);
                    await Mailer.SendAsync(employee.Email, emailSubject, emailBody.Replace("$name", employee.Name));
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch
            {
                throw new Exception("We can't save the modified data.");
            }
            return RedirectToAction("Index");

        }
        #endregion

        #region Index or List methods
        public ActionResult Index(int page = 1)
        {

            var config = AppConfig.Config;
            var employees = db.Employees;
            var histories = db.Histories;
            var pageInfo = _logic.GetPageInfo(config.PerCyclePerson, page);
            var employeePaymentHistory = histories
                                    .Include(n => n.Employee)
                                    .OrderBy(e => e.HistoryID)
                                    .GetPageData(pageInfo).ToList();

            ViewBag.paginationHtml = new MvcHtmlString(Pagination.GetList(pageInfo, url: "?page=@page"));

            return View(employeePaymentHistory);
        }
        #endregion

        #region Details
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
        #endregion

        #region Create
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
        #endregion

        #region Edit
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
        #endregion

        #region Delete
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
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
