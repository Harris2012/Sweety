using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Entity
{
    [ExcelTable("产品表")]
    class ProductEntity
    {
        [ExcelColumn("产品编号")]
        public string ProductNo { get; set; }

        [ExcelColumn("产品名称")]
        public string Name { get; set; }
    }
}
