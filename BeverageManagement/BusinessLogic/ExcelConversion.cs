using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using BeverageManagement.Models.EntityModel;
using DevMvcComponent.Miscellaneous;
using Excel=Microsoft.Office.Interop.Excel;
using System.Web.Mvc;
namespace BeverageManagement.BusinessLogic {
    public  class ExcelConversion {
        private static Excel.Workbook _myBook = null;
        private Excel.Application _myApp;
        public void Open(string localPath) {
            _myApp = new Excel.Application();
            _myApp.Visible = false;
            string path = DirectoryExtension.GetBaseOrAppDirectory() +localPath;
            try {
                _myBook = _myApp.Workbooks.Open(path);
            } catch (Exception ex) {
                
                throw ex;
            }

        }


        public void WriteToExcelFile(IQueryable<History> histories) {
            var mySheet = (Excel.Worksheet) _myBook.Sheets[1]; 
            var currenRow = 1;
            int lastNameRow=2, totalAmount=0;
            //var amountCount = histories
            //    .GroupBy(n => n.EmployeeID)
            //    .Select(g => new { Id = g.Key, Sum = g.Sum(x => x.Amount) }).ToList();
            //var s=amountCount[0].Sum;
            mySheet.UsedRange.ClearContents();
            mySheet.Cells[currenRow, 1] = "Employee Name";
            mySheet.Cells[currenRow, 2] = "Date";
            mySheet.Cells[currenRow, 3] = "Total Paid";
            mySheet.Cells[currenRow, 1].EntireColumn.Font.Bold=true;
            currenRow++;
            var lastEmployeeId = histories.FirstOrDefault().Employee.EmployeeID;
            foreach (var history in histories) {
                totalAmount += history.Amount;
                if (currenRow == 2) 
                {
                    mySheet.Cells[currenRow, 1] = history.Employee.Name;
                    mySheet.Cells[currenRow, 3] = history.Employee.Cycle;
                    lastNameRow = currenRow;
                    totalAmount = history.Amount;
                }
                else if (lastEmployeeId != history.Employee.EmployeeID) 
                {
                    currenRow++;
                    lastEmployeeId = history.Employee.EmployeeID;
                    mySheet.Cells[currenRow, 1] = history.Employee.Name;
                    mySheet.Cells[currenRow, 3] = history.Employee.Cycle;
                    mySheet.Cells[lastNameRow, 3] = totalAmount;
                    lastNameRow = currenRow;
                    totalAmount = history.Amount;
                }
                string debug = history.Dated.ToString("dd-MMM-yy");
               
                mySheet.Cells[currenRow, 2] = debug;
                
                currenRow++;
            }
            mySheet.Cells[lastNameRow, 3] = totalAmount;

            _myBook.Save();
        }

        public void CloseExcelFile() {
            _myBook.Close();
            _myApp.Quit();
        }



    }
}