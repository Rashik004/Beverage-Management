using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BeverageManagement.App_Start;
using DevMvcComponent;
using BeverageManagement.Models.EntityModel;
using DevMvcComponent.Enums;
using DevMvcComponent.Mail;
//using BeverageManagement.BusinessLogic;
namespace BeverageManagement.BusinessLogic
{
    public class MailScheduler : Registry
    {
       // Schedule<MyTask>().ToRunNow().AndEvery(2).Seconds();
        private BeverageManagementEntities db = new BeverageManagementEntities();
        private Logic _logic; 
        public MailScheduler()
        {
            _logic = new Logic(db);
            var config = App.Config;
            bool isCycleChanged;
            var selectedEmployeesForPayment = _logic.GetFinalSelectedEmployeesForCycle(config.PerCyclePerson, config.CurrentRunningCycle, out isCycleChanged);
            //List<string> namesOfSelectedEmployees;
           // namesOfSelecte
            Schedule(() =>
            {
                List<string> namesOfSelectedEmployees=new List<string>();
                foreach (var employee in selectedEmployeesForPayment)
                    namesOfSelectedEmployees.Add(employee.Name);
                var to = "rashikcse2k11@gmail.com";
                var subject = "mails will be sent";
                var body = namesOfSelectedEmployees;
               // string blahblah = namesOfSelectedEmployees.ToString();
                var mailServer = new GmailServer("rhasnatauto@gmail.com", "rashik1234");
                //var gh=~/BusinessLogic/mailToAdmin.html;
                mailServer.QuickSend(to, subject, namesOfSelectedEmployees.ToString());

            }).ToRunNow().AndEvery(50).Seconds(); ;
        }
    }
}