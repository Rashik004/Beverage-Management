using System;
using System.IO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web.Mvc;
using BeverageManagement.Models.EntityModel;
using DevMvcComponent.Pagination;
using BeverageManagement.BusinessLogic;
using BeverageManagement.ViewModel;
using DevMvcComponent;
using DevMvcComponent.Enums;
using DevMvcComponent.Mail;
using DevMvcComponent.Miscellaneous;

namespace BeverageManagement.Controllers {
    [Authorize] // authentication or login : must login
    public class PaymentCyclesController : Controller {
        #region Attributes
        private BeverageManagementEntities db = new BeverageManagementEntities();
        private Logic _logic;
        #endregion

        #region Constructor
        public PaymentCyclesController() {
            _logic = new Logic(db);
        }
        #endregion

        #region Process Payment (GET)
        public ActionResult ProcessPayment() {
            var config = AppConfig.Config;
            bool isCycleChanged;
            var employees = _logic.GetFinalSelectedEmployeesForCycle(config.PerCyclePerson, config.CurrentRunningCycle, out isCycleChanged);
            if (isCycleChanged) {
                config.CurrentRunningCycle++;
                AppConfig.SaveConfig();
            }
            TempData["SelectedEmployees"] = employees;
            return View(employees);
        }
        #endregion

        #region Process Payment (POST)
        [HttpPost, ValidateInput(false)]
        public ActionResult ProcessPayment(EmailDetailViewModel emailInfo) {
            var selectedEmployeesForPayment = (List<Employee>) TempData["SelectedEmployees"];
            emailInfo.EmailBody = emailInfo.EmailBody.Replace("$amount", AppConfig.Config.DefaultBeveragePrice.ToString());

            foreach (var employee in selectedEmployeesForPayment)
            {
                var dbEmployee = db.Employees.FirstOrDefault(e => e.EmployeeID == employee.EmployeeID);
                if(dbEmployee == null)
                    continue;
                dbEmployee.Cycle = employee.Cycle + 1;
                dbEmployee.LastPaymentDate = DateTime.Now;
                if (ModelState.IsValid) {
                    db.Entry(dbEmployee).State = System.Data.Entity.EntityState.Modified;
                    History history = new History();
                    history.EmployeeID = employee.EmployeeID;
                    history.Dated = DateTime.Now;
                    history.WeekNumber = AppConfig.Config.CurrentRunningCycle;
                    history.Amount = (int) AppConfig.Config.DefaultBeveragePrice;
                    db.Histories.Add(history);
                }
            }
            try {
                db.SaveChanges();
            } catch {
                throw new Exception("We can't save the modified data.");
            }
            string timeStamp = DateTime.Now.ToString("dd_MMM_yy_h_mm_ss_tt");
            string folderPath = DirectoryExtension.GetBaseOrAppDirectory() + "ExcelFiles\\";
            var attachmentFilePathAndName = folderPath + timeStamp + ".xls";
            var lastTwoYearsHistories = _logic.GetLastTwoYearsHistories(DateTime.Now);

            ExcelFileCreation(lastTwoYearsHistories, attachmentFilePathAndName);

            #region Thread for mailing and excel deletion
            var thread = new Thread(() => {
                List<Attachment> attachments = new List<Attachment>() { new Attachment(attachmentFilePathAndName) };
                attachments[0].Name = AppConfig.Config.EmailAttachmentName + ".xls";
                string[] employeeEmails = new string[1];
                employeeEmails[0] = selectedEmployeesForPayment.FirstOrDefault().Email;
                MailSendingWrapper mailWrapper = Mvc.Mailer.GetMailSendingWrapper(employeeEmails, emailInfo.EmailSubject, emailInfo.EmailBody, null, attachments, MailingType.MailBlindCarbonCopy);
                                              try
                                              {
                                                  foreach (var employee in selectedEmployeesForPayment)
                                                  {
                                                      employeeEmails[0] = employee.Email;
                                                      mailWrapper = Mvc.Mailer.GetMailSendingWrapper(employeeEmails, emailInfo.EmailSubject, emailInfo.EmailBody.Replace("$name", employee.Name), null, attachments,
                                                          MailingType.MailBlindCarbonCopy);
                                                      Mvc.Mailer.SendMail(mailWrapper, false);
                                                  }
                                              }
                                              catch (Exception ex)
                                              {}
                                              finally
                                              {
                                                  mailWrapper.MailMessage.Dispose();
                                                  mailWrapper.MailServer.Dispose();
                                                  attachments[0] = null;
                                                  attachments = null;
                                                  GC.Collect();
                                                  System.IO.File.Delete(attachmentFilePathAndName);
                                              }


            });
            #endregion

            thread.Start();
            return RedirectToAction("Index");

        }
        #endregion

        private void ExcelFileCreation(IQueryable<History> histories, string excelFilePathAndName) {
            ExcelConversion historyExcelConversion = new ExcelConversion();
            historyExcelConversion.OpenExcelApp();
            historyExcelConversion.InitializeColumnNumbers();
            historyExcelConversion.InitializeHeader();
            historyExcelConversion.BoldColumn(1);
            historyExcelConversion.WriteToExcelFile(histories);
            historyExcelConversion.SaveAsAndQuit(excelFilePathAndName);
            historyExcelConversion.Dispose();
        }
        #region Index or List methods
        public ActionResult Index(int page = 1) {

            var config = AppConfig.Config;
            var employees = db.Employees;
            var histories = db.Histories;
            var pageInfo = _logic.GetPageInfo(config.PerCyclePerson, page);
            var employeePaymentHistory = histories
                                    .Include(n => n.Employee)
                                    .OrderBy(e => e.Dated)
                                    .GetPageData(pageInfo).ToList();

            ViewBag.paginationHtml = new MvcHtmlString(Pagination.GetList(pageInfo, url: "?page=@page", maxNumbersOfPagesShow: 8));

            return View(employeePaymentHistory);
        }
        #endregion

        #region Details
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null) {
                return HttpNotFound();
            }
            return View(employee);
        }
        #endregion


        #region Dispose
        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
