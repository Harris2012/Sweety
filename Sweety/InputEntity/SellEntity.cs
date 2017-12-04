using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.InputEntity
{
    [ExcelTable("本期销项明细")]
    class SellEntity
    {
        [ExcelColumn("销售合同号", 1)]
        public string ProductNo { get; set; }

        [ExcelColumn("商品数量", 4)]
        public string Name { get; set; }
    }
}
