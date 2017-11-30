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

        [ExcelColumn("产品采购数", 4)]
        public int BuyCount { get; set; }

        [ExcelColumn("产品销售数", 5)]
        public int SellCount { get; set; }

        [ExcelColumn("产品总采购数", 6)]
        public int ProductBuyCount { get; set; }

        [ExcelColumn("产品总销售数", 7)]
        public int ProductSellCount { get; set; }

        [ExcelColumn("产品总采购次数", 8)]
        public int ProductBuyTime { get; set; }

        [ExcelColumn("产品总销售次数", 9)]
        public int ProductSellTime { get; set; }

        [ExcelColumn("产品总剩余", 10)]
        public int ProductTotalRemain { get; set; }

        [ExcelColumn("产品总不足", 11)]
        public int ProductTotalRequire { get; set; }

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
