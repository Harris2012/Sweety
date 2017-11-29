using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Entity
{
    [ExcelTable("简单报表")]
    class TargetEntity
    {
        [ExcelColumn("交易号", 1)]
        public string BusinessNo { get; set; }

        [ExcelColumn("单据号", 2)]
        public string PaperNo { get; set; }

        [ExcelColumn("产品编号", 3)]
        public string ProductNo { get; set; }

        [ExcelColumn("采购数", 4)]
        public int BuyCount { get; set; }

        [ExcelColumn("销售数", 5)]
        public int SellCount { get; set; }
    }
}
