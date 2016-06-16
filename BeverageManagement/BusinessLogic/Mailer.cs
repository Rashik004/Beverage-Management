using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using BeverageManagement.Models.EntityModel;
using BeverageManagement.ViewModel;
using DevMvcComponent;
using DevMvcComponent.Enums;
using DevMvcComponent.Miscellaneous;

namespace BeverageManagement.BusinessLogic {
    public class Mailer {
        public EmailDetailViewModel EmailDetail { get; set; }
        private static List<Attachment> _attachments=new List<Attachment>(){new Attachment(DirectoryExtension.GetBaseOrAppDirectory() + "Content/new.xlsx")};
        /// <summary>
        /// Sends mail to one person
        /// </summary>
        /// <param name="mailTo">receiver's mailing address</param>
        /// <param name="receiverName">receiver's name</param>
        private void SendOneMail(string mailTo, string receiverName) {
            Mvc.Mailer.QuickSend(mailTo, EmailDetail.EmailSubject, EmailDetail.EmailBody.Replace("$name", receiverName), MailingType.RegularMail, true, _attachments);
            
        }
        /// <summary>
        /// Sends mail to all the employees given in the parameter
        /// </summary>
        /// <param name="selectedEmployeesForMailing"></param>
        public void sendMailToAll(List<Employee> selectedEmployeesForMailing) {
            foreach (var employee in selectedEmployeesForMailing) {
                SendOneMail(employee.Email, employee.Name);
            }
        }
    }
}