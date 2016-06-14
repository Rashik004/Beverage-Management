using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Excel=Microsoft.Office.Interop.Excel;
namespace BeverageManagement.BusinessLogic {
    public  class ExcelConversion {
        public  string Path;
        private static Excel.Workbook MyBook = null; 

        public void test() {
            var MyApp = new Excel.Application();
            Path = "E:/Coding Practice/Beverage-Management/BeverageManagement/Content/new.xlsx";
            var p = "new.xlsx";
            MyApp.Visible = false;
            MyBook = MyApp.Workbooks.Open(Path);
            var MySheet = (Excel.Worksheet) MyBook.Sheets[1]; // Explicit cast is not required here
            var lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            MySheet.Cells[lastRow, 1] = "rhasnat";
            var s=Path;
            MyBook.Save();
            MyBook.Close();
        }


    }
}