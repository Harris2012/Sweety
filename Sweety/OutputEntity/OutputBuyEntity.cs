using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.OutputEntity
{
    [ExcelTable("本期进项明细")]
    class OutputBuyEntity
    {
        [ExcelColumn("序号", 1)]
        public string Id { get; set; }

        [ExcelColumn("货号", 2)]
        public string GoodsNo { get; set; }

        [ExcelColumn("采购合同号", 3)]
        public string CaiGouContractNo { get; set; }

        [ExcelColumn("编号", 4)]
        public string BianHao { get; set; }

        [ExcelColumn("销方名称", 5)]
        public string XiaoFangMinCheng { get; set; }

        [ExcelColumn("商品名称", 6)]
        public string GoodsName { get; set; }

        [ExcelColumn("组别", 7)]
        public string GroupCategory { get; set; }

        [ExcelColumn("收票号码", 8)]
        public string BillingNo { get; set; }

        [ExcelColumn("收票号码", 9)]
        public float BillingMoney { get; set; }

        [ExcelColumn("不含税金额", 10)]
        public float NoTaxTotalMoney { get; set; }

        [ExcelColumn("税额", 11)]
        public float TaxMoney { get; set; }

        [ExcelColumn("收票吨数", 12)]
        public float BillingCount { get; set; }

        [ExcelColumn("商品含税单价", 13)]
        public float GoodsUnitMoney { get; set; }

        [ExcelColumn("类型", 14)]
        public string Mode { get; set; }
    }
}
