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
        public string SellContractNo { get; set; }

        [ExcelColumn("购货单位", 2)]
        public string BuProductOrganization { get; set; }

        [ExcelColumn("商品名称", 3)]
        public string ProductName { get; set; }

        [ExcelColumn("商品数量", 4)]
        public float ProductCount { get; set; }

        [ExcelColumn("商品含税单价", 5)]
        public float ProductUnitPriceWithTax { get; set; }

        [ExcelColumn("商品含税金额", 6)]
        public float ProductMoneyWithTax { get; set; }

        [ExcelColumn("发票号", 7)]
        public string BillNo { get; set; }

    }
}
