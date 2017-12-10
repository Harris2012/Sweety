using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class ExcelColumnAttribute : Attribute
    {
        public string Name { get; private set; }

        public int ColumnIndex { get; private set; }

        /// <summary>
        /// 从Excel中读到的列序号
        /// </summary>
        public int ColumnIndexFromExcel { get; set; }

        public ExcelColumnAttribute(string name, int columnIndex)
        {
            this.Name = name;
            this.ColumnIndex = columnIndex;
        }

        /// <summary>
        /// 单元格格式
        /// </summary>
        public ExcelCellFormat ExcelCellFormat { get; set; }
    }
}
