using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class ExcelColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public ExcelColumnAttribute(string name)
        {
            this.Name = name;
        }
    }
}
