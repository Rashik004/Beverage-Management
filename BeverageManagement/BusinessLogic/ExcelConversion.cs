using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using DevMvcComponent.Miscellaneous;
using Excel=Microsoft.Office.Interop.Excel;
namespace BeverageManagement.BusinessLogic {
    public  class ExcelConversion {
        public  string Path;
        private static Excel.Workbook MyBook = null; 

        public void Test() {
            var myApp = new Excel.Application();
            Path = "E:/Coding Practice/Beverage-Management/BeverageManagement/Content/new.xlsx";
            string p = DirectoryExtension.GetBaseOrAppDirectory()+"Content/new.xlsx";
            myApp.Visible = false;
            try {
                MyBook = myApp.Workbooks.Open(p);
            } catch(Exception ex) {

                throw ex;
            }
            var mySheet = (Excel.Worksheet) MyBook.Sheets[1]; // Explicit cast is not required here
            var lastRow = mySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            mySheet.Cells[lastRow, 2] = "Hoina keno?? :'(";
            var s=Path;
            MyBook.Save();
            MyBook.Close();
        }


    }
}