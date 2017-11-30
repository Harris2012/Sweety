using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Entity
{
    [ExcelTable("基础数据表")]
    public class SourceEnity
    {
        [ExcelColumn("单据号", 1)]
        public string PaperNo { get; set; }

        [ExcelColumn("交易号", 2)]
        public string BusinessNo { get; set; }

        [ExcelColumn("商品编号", 3)]
        public string ProductNo { get; set; }

        /// <summary>
        /// 采购 或 销售
        /// </summary>
        [ExcelColumn("类型", 4)]
        public string Mode { get; set; }

        [ExcelColumn("数量", 5)]
        public int Count { get; set; }

        [ExcelColumn("日期", 6)]
        public DateTime CreateTime { get; set; }
    }
}
