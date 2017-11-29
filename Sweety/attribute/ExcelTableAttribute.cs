using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class ExcelTableAttribute : Attribute
    {
        public string Name { get; private set; }

        public ExcelTableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
