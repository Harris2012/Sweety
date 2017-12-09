using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sweety
{
    class ColumnInfo
    {
        public PropertyInfo PropertyInfo { get; set; }

        public ExcelColumnAttribute ColumnAttribute { get; set; }
    }
}
