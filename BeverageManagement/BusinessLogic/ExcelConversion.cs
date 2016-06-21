using System;
using System.Linq;
using BeverageManagement.Models.EntityModel;
using Excel=Microsoft.Office.Interop.Excel;
namespace BeverageManagement.BusinessLogic {
    public  class ExcelConversion {
        #region Attributes
        private Excel.Workbook _myBook;
        private Excel.Application _myApp;
        private object _misValue;
        private int _employeeNameColumn;
        private int _dateColumn;
        private int _amountOfPaymentColumn;
        private int _numberOfPaymentColumn; 
        #endregion

        public void OpenExcelApp() {
            _misValue = System.Reflection.Missing.Value;
            _myApp = new Excel.Application();
            _myBook = _myApp.Workbooks.Add(_misValue);
        }

        public void InitializeColumnNumbers(int employeeNameColumn=1, int dateColumn=2, int amountOfPaymentColumn=3, int numberOfPaymentColumn=4) {
            _employeeNameColumn = employeeNameColumn;
            _dateColumn = dateColumn;
            _amountOfPaymentColumn = amountOfPaymentColumn;
            _numberOfPaymentColumn = numberOfPaymentColumn;
        }

        public void InitializeHeader(string column1Header = "Employee Name", string column2Header = "Date", string column3Header = "Amount of Payment", string column4Header = "Number of payment") {
            var currentWorksheet = (Excel.Worksheet) _myBook.Sheets[1]; 

            currentWorksheet.Cells[1, _employeeNameColumn] = column1Header;
            currentWorksheet.Cells[1, _dateColumn] = column2Header;
            currentWorksheet.Cells[1, _amountOfPaymentColumn] = column3Header;
            currentWorksheet.Cells[1, _numberOfPaymentColumn] = column4Header;
        }

        public void BoldColumn(int column) {
            var currentWorksheet = (Excel.Worksheet) _myBook.Sheets[1]; 
            currentWorksheet.Cells[1, column].EntireColumn.Font.Bold = true;
            
        }

        public void WriteToExcelFile(IQueryable<History> histories) {
            var mySheet = (Excel.Worksheet) _myBook.Sheets[1]; 
            var currenRow = 2;

            var firstOrDefault = histories.FirstOrDefault();
            if (firstOrDefault != null) {
                var lastEmployeeId = firstOrDefault.Employee.EmployeeID;
            
                foreach (var history in histories) {
                    if (currenRow == 2) 
                    {
                        mySheet.Cells[currenRow, _employeeNameColumn] = history.Employee.Name;
                        mySheet.Cells[currenRow, _numberOfPaymentColumn] = history.Employee.Cycle;
                    }
                    else if (lastEmployeeId != history.Employee.EmployeeID) 
                    {
                        currenRow++;
                        lastEmployeeId = history.Employee.EmployeeID;
                        mySheet.Cells[currenRow, _employeeNameColumn] = history.Employee.Name;
                        mySheet.Cells[currenRow, _numberOfPaymentColumn] = history.Employee.Cycle;
                    }

                    string debug = history.Dated.ToString("dd-MMM-yy");
                    mySheet.Cells[currenRow, _dateColumn] = debug;
                    mySheet.Cells[currenRow, _amountOfPaymentColumn] = history.Amount;
                
                    currenRow++;
                }
            }
        }

        public void SaveAsAndQuit(string excelFilePathAndName) {
            _myBook.SaveAs(excelFilePathAndName, Excel.XlFileFormat.xlWorkbookNormal, _misValue, _misValue, _misValue, _misValue, Excel.XlSaveAsAccessMode.xlExclusive, _misValue, _misValue, _misValue, _misValue, _misValue);
            foreach (Excel.Workbook workbook in _myApp.Workbooks) {
                workbook.Close(0);
            }
            _myApp.Quit();
            _myApp = null;

        }

        public void Dispose() {

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

    }
}