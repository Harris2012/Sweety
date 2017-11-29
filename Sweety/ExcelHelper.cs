﻿using Sweety.Entity;
using System;
using System.Collections.Concurrent;
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

        /// <summary>
        /// Key: ExcelFilePath
        /// Value: Excel.WorkBook
        /// </summary>
        private static readonly ConcurrentDictionary<string, Excel.Workbook> bookMap = new ConcurrentDictionary<string, Excel.Workbook>();

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

        private static Excel.Workbook GetWorkBook(Excel.Application app, string excelFilePath)
        {
            return bookMap.GetOrAdd(excelFilePath, v =>
            {
                return app.Workbooks.Open(excelFilePath);
            });
        }

        private static void CloseWorkBook(string excelFilePath)
        {
            Excel.Workbook book;
            if (bookMap.TryRemove(excelFilePath, out book))
            {
                book.Close();
            }
        }

        public static List<T> ReadFromExcel<T>(string excelFilePath) where T : class, new()
        {
            List<T> entityList = new List<T>();

            var type = typeof(T);
            ExcelTableAttribute tableAttribute = (ExcelTableAttribute)type.GetCustomAttributes(typeof(ExcelTableAttribute), false)[0];
            var tableName = tableAttribute.Name;
            var properties = type.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (string.IsNullOrEmpty(tableName) || properties.Length == 0)
            {
                return entityList;
            }

            Excel.Application app = GetApplication();
            Excel.Workbook book = GetWorkBook(app, excelFilePath);
            for (int sheetIndex = 1; sheetIndex <= book.Sheets.Count; sheetIndex++)
            {
                Excel.Worksheet sheet = book.Sheets[sheetIndex];
                if (sheet.Name.Equals(tableName))
                {
                    var maxRow = GetMaxRow(sheet);
                    var maxColumn = GetMaxColumn(sheet);

                    Excel.Range range = sheet.Range[ToColumnLabel(1) + 1, ToColumnLabel(maxColumn) + maxRow];

                    for (int row = 1; row <= maxRow; row++)
                    {
                        T entity = new T();
                        foreach (var property in type.GetProperties())
                        {
                            ExcelColumnAttribute columnAttribute = (ExcelColumnAttribute)property.GetCustomAttributes(typeof(ExcelColumnAttribute), false)[0];

                            Excel.Range cellRange = range[row, columnAttribute.ColumnIndex];

                            string cellText = cellRange.Text;

                            switch (property.PropertyType.ToString())
                            {
                                case "System.Int32":
                                    {
                                        int intValue = 0;
                                        if (int.TryParse(cellRange.Text, out intValue))
                                        {
                                            property.SetValue(entity, intValue, null);
                                        }
                                    }
                                    break;
                                case "System.Int64":
                                    {
                                        long longValue = 0;
                                        if (long.TryParse(cellRange.Text, out longValue))
                                        {
                                            property.SetValue(entity, longValue, null);
                                        }
                                    }
                                    break;
                                case "System.String":
                                    {
                                        property.SetValue(entity, cellRange.Text, null);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        entityList.Add(entity);
                    }
                    break;
                }
            }

            CloseWorkBook(excelFilePath);

            return entityList;
        }

        private static int GetMaxRow(Excel.Worksheet sheet)
        {
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

            return maxRow;
        }

        private static int GetMaxColumn(Excel.Worksheet sheet)
        {
            int maxColumn = 0;

            for (int column = 1; ; column++)
            {
                Excel.Range range = sheet.Cells[1, column];
                if (string.IsNullOrEmpty(range.Text))
                {
                    break;
                }
                maxColumn = column;
            }

            return maxColumn;
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

        public static void WriteToExcel(string excelFilePath, List<ProductEntity> entityList)
        {
            foreach (var entity in entityList)
            {

            }
        }
    }
}
