using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.OutputEntity
{
    [ExcelTable("本期销项明细")]
    class OutputSellEntity
    {
        [ExcelColumn("货号", 1)]
        public string GoodsNo { get; set; }

        [ExcelColumn("销售合同号", 2)]
        public string ContractNo { get; set; }

        [ExcelColumn("购货单位", 3)]
        public string GouHuoDanWei { get; set; }

        [ExcelColumn("商品名称", 4)]
        public string GoodsName { get; set; }

        [ExcelColumn("商品数量", 5)]
        public float GoodsCount { get; set; }

        [ExcelColumn("商品含税单价", 6)]
        public float GoodsUnitMoney { get; set; }

        [ExcelColumn("商品含税金额", 7)]
        public float GoodsTaxTotalMoney { get; set; }

        [ExcelColumn("发票号", 8)]
        public string BillingNo { get; set; }

        [ExcelColumn("进项收票吨数", 9)]
        public double BuyCount { get; set; }

        [ExcelColumn("库存", 10)]
        public double RemainCount { get; set; }

        [ExcelColumn("销售类型", 11)]
        public string SaleMode { get; set; }

        [ExcelColumn("采购单位", 12)]
        public string CaiGouDanWei { get; set; }

        [ExcelColumn("采购单价", 13)]
        public float CaiGouUnitMoney { get; set; }

        [ExcelColumn("暂估采购总金额", 14)]
        public float ZanGuCaiGouTotalMoney { get; set; }

        [ExcelColumn("采购合同", 15)]
        public string CaiGouContract { get; set; }

        [ExcelColumn("暂估采购不含税金额", 16)]
        public float ZanGuCaiGouNoTaxTotalMoney { get; set; }
    }
}
