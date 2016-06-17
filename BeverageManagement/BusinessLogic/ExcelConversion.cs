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
        //public void Open(string localPath) {
        //    _myApp = new Excel.Application();
        //    _myApp.Visible = false;
        //    string path = DirectoryExtension.GetBaseOrAppDirectory() +localPath;
        //    try {
        //        _myBook = _myApp.Workbooks.Open(path);
        //    } catch (Exception ex) {
                
        //        throw ex;
        //    }

        //}
        private Excel.Workbook _myBook;
        private Excel.Application _myApp;
        public void WriteToExcelFile(IQueryable<History> histories, string excelFilePathAndName) {

            object misValue = System.Reflection.Missing.Value;
            _myApp = new Excel.Application();
            _myBook= _myApp.Workbooks.Add(misValue);
            var mySheet = (Excel.Worksheet) _myBook.Sheets[1]; 
            var currenRow = 1;
            var lastEmployeeId = histories.FirstOrDefault().Employee.EmployeeID;
            int lastNameRow = 2, totalAmount = 0, numberOfPayment = 0;
            
            mySheet.UsedRange.ClearContents();
            mySheet.Cells[currenRow, 1] = "Employee Name";
            mySheet.Cells[currenRow, 2] = "Date";
            mySheet.Cells[currenRow, 3] = "Total Paid";
            mySheet.Cells[currenRow, 4] = "Number of payment";
            mySheet.Cells[currenRow, 1].EntireColumn.Font.Bold=true;
            currenRow++;
            
            foreach (var history in histories) {
                numberOfPayment++;
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
                    mySheet.Cells[lastNameRow, 3] = totalAmount-history.Amount;
                    mySheet.Cells[lastNameRow, 4] = numberOfPayment-1;
                    numberOfPayment = 1;
                    lastNameRow = currenRow;
                    totalAmount = history.Amount;
                }

                string debug = history.Dated.ToString("dd-MMM-yy");
                mySheet.Cells[currenRow, 2] = debug;
                
                currenRow++;
            }
            mySheet.Cells[lastNameRow, 3] = totalAmount;
            mySheet.Cells[lastNameRow, 4] = numberOfPayment;
            _myBook.SaveAs(excelFilePathAndName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            _myBook.Close(true, misValue, misValue);
            _myApp.Quit();

        }



        public void closeFile() {
            foreach (Excel.Workbook _workbook in _myApp.Workbooks) {
                _workbook.Close(0);
            }
            _myApp = null;
            var process = System.Diagnostics.Process.GetProcessesByName("Excel");
            foreach (var p in process) {
                if (!string.IsNullOrEmpty(p.ProcessName)) {
                    try {
                        p.Kill();
                    } catch (Exception ex) {
                        throw ex;
                    }
                }
            }
            GC.Collect();
        }

        private void releaseObject(object obj) {
            try {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            } catch (Exception ex) {
                obj = null;
            } finally {
                GC.Collect();
            }
        }

    }
}