using DevMvcComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BeverageManagement.BusinessLogic
{
    public static class Mailer
    {
        public static async Task SendAsync(string to, string subject, string body)
        {
            await Task.Factory.StartNew(() =>
            {
                Mvc.Mailer.QuickSend(to, subject, body, isAsync: false);
            });
        }
    }
}