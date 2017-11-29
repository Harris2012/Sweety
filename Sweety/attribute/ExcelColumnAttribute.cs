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

        public ExcelColumnAttribute(string name, int columnIndex)
        {
            this.Name = name;
            this.ColumnIndex = columnIndex;
        }
    }
}
