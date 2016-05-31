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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static async Task SendAsync(string to, string subject, string body)
        {
            await Task.Factory.StartNew(() =>
            {
                Mvc.Mailer.QuickSend(to, subject, body, isAsync: false);
            });
        }
    }
}