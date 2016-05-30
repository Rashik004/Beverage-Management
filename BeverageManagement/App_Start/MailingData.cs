using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeverageManagement.Models
{
    public class MailingData
    {
        public string senderEmail;
        public string senderPassword;
        MailingData (string email="rhasnatauto@gmail.com", string password="rashik1234")
        {
            senderEmail = email;
            senderPassword = password;
        }

    }
}