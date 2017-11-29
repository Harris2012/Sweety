using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Entity
{
    /// <summary>
    /// 原始数据表
    /// </summary>
    public class OriginEntity
    {
        /// <summary>
        /// 交易号
        /// </summary>
        [ExcelColumn("交易号")]
        public string BusinessNo { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        [ExcelColumn("单据号")]
        public string PaperNo { get; set; }

        [ExcelColumn("产品编号")]
        public string ProductNo { get; set; }

        [ExcelColumn("采购数")]
        public int BuyCount { get; set; }

        [ExcelColumn("销售数")]
        public int SellCount { get; set; }
    }
}
