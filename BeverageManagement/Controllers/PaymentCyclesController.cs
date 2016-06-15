using System;
using System.Collections.Generic;
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
using DevMvcComponent.HtmlEnhancements;

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
        public ActionResult ProcessPayment(EmailDetailViewModel emailInfo)
        {

            var config = AppConfig.Config;
            var emailSubject = emailInfo.EmailSubject;
            var emailBody = emailInfo.EmailBody;
            var selectedEmployeesForPayment = (List<Employee>)TempData["SelectedEmployees"];
            emailBody = emailBody.Replace("$amount", AppConfig.Config.DefaultBeveragePrice.ToString());
            foreach (var employee in selectedEmployeesForPayment)
            {
                employee.Cycle=employee.Cycle+1;
                if (ModelState.IsValid)
                {
                    db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                    History history = new History();
                    history.EmployeeID = employee.EmployeeID;
                    history.Dated = DateTime.Now;
                    history.WeekNumber = AppConfig.Config.CurrentRunningCycle;
                    history.Amount = (int)AppConfig.Config.DefaultBeveragePrice;
                    db.Histories.Add(history);
                    Mvc.Mailer.QuickSend(employee.Email, emailSubject, emailBody.Replace("$name", employee.Name));
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

            var lastTwoYearsHistories = _logic.GetLastTwoYearsHistories(DateTime.Now);
            var s=lastTwoYearsHistories.Count();
            ExcelConversion emailAttachment = new ExcelConversion();

            try {
                emailAttachment.Open("Content/new.xlsx");
            } catch (Exception ex) {
                
                throw ex;
            }

            emailAttachment.WriteToExcelFile(lastTwoYearsHistories);
            emailAttachment.CloseExcelFile();
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
                                    .OrderBy(e => e.Dated)
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
