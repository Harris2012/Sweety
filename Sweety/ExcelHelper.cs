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

                int maxColumn = 0;
                int maxRow = 0;
                for (int row = 1; ; row++)
                {
                    Excel.Range range = sheet.Cells[row, 1];
                    if (string.IsNullOrEmpty(range.Text))
                    {
                        break;
                    }
                    maxRow = row;
                }
                for (int column = 1; ; column++)
                {
                    Excel.Range range = sheet.Cells[1, column];
                    if (string.IsNullOrEmpty(range.Text))
                    {
                        break;
                    }
                    maxColumn = column;
                }

                table.MaxRow = maxRow;
                table.MaxColumn = maxColumn;

                returnValue.Add(table);
            }

            return returnValue;
        }

        private static string ToColumnLabel(int column)
        {
            List<char> list = new List<char>();
            while (column > 0)
            {
                var value = column % 26;

                if (value > 0)
                {
                    char ch = (char)(value + 64);

                    list.Insert(0, ch);
                }
                column = column / 26;
            }

            return string.Join(string.Empty, list);
        }

        public static void WriteToExcel(string excelFilePath, Dictionary<string, List<ExcelTable>> entityList)
        {
            foreach (var entity in entityList)
            {

            }
        }
    }
}
