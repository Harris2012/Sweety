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

        [ExcelColumn("总进货数", 6)]
        public int TotalBuyCount { get; set; }

        [ExcelColumn("总进货次数", 7)]
        public int TotalBuyTime { get; set; }

        [ExcelColumn("总销货次数", 8)]
        public int TotalSellTime { get; set; }

        [ExcelColumn("总销货数", 9)]
        public int TotalSellCount { get; set; }

        [ExcelColumn("总剩余库存", 10)]
        public int TotalRemain { get; set; }

        [ExcelColumn("总不足库存", 11)]
        public int TotalRequire { get; set; }

        /// <summary>
        /// 直运
        /// 暂估
        /// 结算前期库存
        /// </summary>
        [ExcelColumn("销售模式", 12)]
        public string SaleMode { get; set; }

        [ExcelColumn("备注", 13)]
        public string Remark { get; set; }

        [ExcelColumn("关联单据号", 14)]
        public string RelatedPaper { get; set; }

        /// <summary>
        /// 关联单据号
        /// </summary>
        private List<string> relatedPapers = new List<string>();

        public void AddRelatedPaper(string paperNo)
        {
            this.relatedPapers.Add(paperNo);

            this.RelatedPaper = string.Join(",", relatedPapers);
        }
    }
}
