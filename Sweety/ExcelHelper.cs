using Sweety.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sweety
{
    class ExcelHelper
    {
        private static Excel.Application coreApp;
        private static readonly object lockObject = new object();

        public static void Stop()
        {
            if (coreApp != null)
            {
                coreApp.Quit();
            }
        }

        private static Excel.Application GetApplication()
        {
            if (coreApp == null)
            {
                lock (lockObject)
                {
                    if (coreApp == null)
                    {
                        coreApp = new Excel.Application();
                        coreApp.Visible = false;
                        coreApp.DisplayAlerts = false;
                        coreApp.ScreenUpdating = false;
                    }
                }
            }

            return coreApp;
        }


        public static List<ExcelTable> ReadFromExcel(string excelFilePath)
        {
            List<ExcelTable> returnValue = new List<ExcelTable>();

            Excel.Application app = GetApplication();

            Excel.Workbook book = app.Workbooks.Open(excelFilePath);

            for (int sheetIndex = 1; sheetIndex <= book.Sheets.Count; sheetIndex++)
            {
                Excel.Worksheet sheet = book.Sheets[sheetIndex];

                ExcelTable table = new ExcelTable();
                table.Name = sheet.Name;

                returnValue.Add(table);
            }

            return returnValue;
        }

        public static void WriteToExcel(string excelFilePath, Dictionary<string, List<ExcelTable>> entityList)
        {
            foreach (var entity in entityList)
            {

            }
        }
    }
}
