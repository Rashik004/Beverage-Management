using System;
using System.Web.Mvc;

namespace BeverageManagement.ViewModel {
    public class HistoryByDateViewModel {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public string Active { get; set; }

        public MvcHtmlString GetDisplay() {
            var result ="<span class=\"badge\">" + Count + "</span>  " + Date.ToString("dd-MMM-y");
            return new MvcHtmlString(result);
        }
    }
}