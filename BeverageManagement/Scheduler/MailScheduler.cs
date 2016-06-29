using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevMvcComponent;
using BeverageManagement.Models.EntityModel;
using DevMvcComponent.Enums;
using DevMvcComponent.Mail;
using BeverageManagement.Scheduler;
//using BeverageManagement.BusinessLogic;
namespace BeverageManagement.BusinessLogic
{
    public class MailScheduler : Registry
    { 
        public MailScheduler() {
            Schedule<MailingJob>().ToRunEvery(1).Weeks().On(DayOfWeek.Thursday).At(12,0);
        }
    }
}