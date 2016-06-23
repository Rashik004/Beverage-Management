using DevMvcComponent;
using FluentScheduler;
using System.Linq;
using System.Web.Hosting;
using BeverageManagement.BusinessLogic;
using BeverageManagement.Models.EntityModel;
namespace BeverageManagement.Scheduler {
    public class MailingJob : IJob, IRegisteredObject {
        private readonly object _lock = new object();
        private Logic _logic;

        private bool _shuttingDown;

        public MailingJob() {
            // Register this job with the hosting environment.
            // Allows for a more graceful stop of the job, in the case of IIS shutting down.
            HostingEnvironment.RegisterObject(this);
        }

        public void Execute() {
            lock (_lock) {
                if (_shuttingDown)
                    return;
                _logic = new Logic(new BeverageManagementEntities());
                //int a = ;
                bool over = true;
                var selectedEmployeesForPayment = _logic.GetFinalSelectedEmployeesForCycle(AppConfig.Config.PerCyclePerson, AppConfig.Config.CurrentRunningCycle, out over);
                var names = selectedEmployeesForPayment.Select(n => n.Name).ToList();
                var commaSeperatedNames = "";
                foreach (var name in names) {
                    if (commaSeperatedNames.Length != 0)
                        commaSeperatedNames += ", ";
                    commaSeperatedNames += name;
                }
                Mvc.Mailer.QuickSend("rashikcse2k11@gmail.com", "hi!! mail sending reminder", "we'll be sending mails to the employees(" + commaSeperatedNames+") selected for beverage payment this week.", isAsync: false);
                // Do work, son!
            }
        }

        public void Stop(bool immediate) {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock) {
                _shuttingDown = true;
            }

            HostingEnvironment.UnregisterObject(this);
        }
    }
}