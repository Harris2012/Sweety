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

        /// <summary>
        /// 直运
        /// 暂估
        /// 结算前期库存
        /// </summary>
        [ExcelColumn("销售模式", 6)]
        public string SaleMode { get; set; }

        [ExcelColumn("备注", 7)]
        public string Remark { get; set; }

        [ExcelColumn("本月总进货数(同一交易号)", 8)]
        public int TotalBuyCount { get; set; }

        [ExcelColumn("本月总进货次数(同一交易号)", 9)]
        public int TotalBuyTime { get; set; }

        [ExcelColumn("本月总销货数(同一交易号)", 10)]
        public int TotalSellCount { get; set; }

        [ExcelColumn("本月总销货次数(同一交易号)", 11)]
        public int TotalSellTime { get; set; }

        [ExcelColumn("本月总剩余库存(同一交易号)", 12)]
        public int TotalRemain { get; set; }

        [ExcelColumn("本月总不足库存(同一交易号)", 13)]
        public int TotalRequire { get; set; }
    }
}
