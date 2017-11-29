using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety
{
    public class ExcelTable
    {
        public string Name { get; set; }

        public int MaxRow { get; set; }

        public int MaxColumn { get; set; }

        public object[,] cells { get; set; }
    }
}
