using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeverageManagement.Models
{
    public class MailingData
    {
        public string SenderEmail;
        public string SenderPassword;
        MailingData (string email="rhasnatauto@gmail.com", string password="rashik1234")
        {
            SenderEmail = email;
            SenderPassword = password;
        }

    }
}