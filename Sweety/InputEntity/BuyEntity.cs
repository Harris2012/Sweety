using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.InputEntity
{
    [ExcelTable("本期进项明细")]
    class BuyEntity
    {
        [ExcelColumn("货号", 1)]
        public string GoodsNo { get; set; }

        [ExcelColumn("单据号", 11)]
        public float 收票吨数 { get; set; }
    }
}
