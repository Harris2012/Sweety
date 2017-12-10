using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.OutputEntity
{
    [ExcelTable("本期进项明细")]
    class OutputBuyEntity
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
        public string GoodsNo { get; set; }

        /// <summary>
        /// 采购合同号
        /// </summary>
        [ExcelColumn("采购合同号", 3)]
        public string CaiGouContractNo { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [ExcelColumn("编号", 4)]
        public string BianHao { get; set; }

        /// <summary>
        /// 销方名称
        /// </summary>
        [ExcelColumn("销方名称", 5)]
        public string XiaoFangMinCheng { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [ExcelColumn("商品名称", 6)]
        public string ProductName { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        [ExcelColumn("组别", 7)]
        public string ZuBie { get; set; }

        /// <summary>
        /// 收票号码
        /// </summary>
        [ExcelColumn("收票号码", 8)]
        public string ShouPiaoHaoMa { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        [ExcelColumn("发票金额", 9, ExcelCellFormat = ExcelCellFormat.Number2)]
        public decimal FaPiaoJinE { get; set; }

        /// <summary>
        /// 不含税金额
        /// </summary>
        [ExcelColumn("不含税金额", 10)]
        public decimal BuHanShuiJinE { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        [ExcelColumn("税额", 11, ExcelCellFormat = ExcelCellFormat.Number2)]
        public decimal TaxMoney { get; set; }

        /// <summary>
        /// 收票吨数
        /// </summary>
        [ExcelColumn("收票吨数", 12)]
        public double ReceiveTicketCount { get; set; }

        /// <summary>
        /// 商品含税单价
        /// </summary>
        [ExcelColumn("商品含税单价", 13)]
        public decimal ShangPinHanShuiDanJia { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [ExcelColumn("类型", 14)]
        public string SellMode { get; set; }
    }
}
