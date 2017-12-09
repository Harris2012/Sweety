using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.InputEntity
{
    [ExcelTable("本期进项明细")]
    class BuyEntity
    {
        /// <summary>
        /// 序号
        /// </summary>
        [ExcelColumn("序号", 1)]
        public string Id { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        [ExcelColumn("货号", 2)]
        public string ProductNo { get; set; }

        /// <summary>
        /// 采购合同号
        /// </summary>
        [ExcelColumn("采购合同号", 3)]
        public string BuyContractNo { get; set; }

        /// <summary>
        /// Number
        /// </summary>
        [ExcelColumn("编号", 4)]
        public string Number { get; set; }

        /// <summary>
        /// 销方名称
        /// </summary>
        [ExcelColumn("销方名称", 5)]
        public string 销方名称 { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [ExcelColumn("商品名称", 6)]
        public string ProductName { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        [ExcelColumn("组别", 7)]
        public string 组别 { get; set; }

        /// <summary>
        /// 收票号码
        /// </summary>
        [ExcelColumn("收票号码", 8)]
        public string BillNo { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        [ExcelColumn("发票金额", 9)]
        public float BillMoney { get; set; }

        /// <summary>
        /// 不含税金额
        /// </summary>
        [ExcelColumn("不含税金额", 10)]
        public float WithoutTaxMoney { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        [ExcelColumn("税额", 11)]
        public float TaxMoney { get; set; }

        /// <summary>
        /// 收票吨数
        /// </summary>
        [ExcelColumn("收票吨数", 12)]
        public float ReceiveTicketCount { get; set; }

    }
}
