using BeverageManagement.Models.EntityModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeverageManagement.ViewModel
{
    public class EmailDetails
    {
        public string emailSubject { get; set; }
       // [Html]
        public string emailBody { get; set; }
    }
}