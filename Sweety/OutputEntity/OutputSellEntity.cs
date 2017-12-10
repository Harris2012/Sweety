using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.OutputEntity
{
    [ExcelTable("本期销项明细")]
    class OutputSellEntity
    {
        /// <summary>
        /// 货号
        /// </summary>
        [ExcelColumn("货号", 1)]
        public string ProductNo { get; set; }

        /// <summary>
        /// 销售合同号
        /// </summary>
        [ExcelColumn("销售合同号", 2)]
        public string XiaoShouContractNo { get; set; }

        /// <summary>
        /// 购货单位
        /// </summary>
        [ExcelColumn("购货单位", 3)]
        public string GouHuoDanWei { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [ExcelColumn("商品名称", 4)]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [ExcelColumn("商品数量", 5)]
        public double ProductCount { get; set; }

        /// <summary>
        /// 商品含税单价
        /// </summary>
        [ExcelColumn("商品含税单价", 6)]
        public decimal ShangPinHanShuiDanJia { get; set; }

        /// <summary>
        /// 商品含税金额
        /// </summary>
        [ExcelColumn("商品含税金额", 7, ExcelCellFormat = ExcelCellFormat.Number2)]
        public decimal ShangPinHanShuiJinE { get; set; }

        /// <summary>
        /// 发票号
        /// </summary>
        [ExcelColumn("发票号", 8)]
        public string FaPiaoHao { get; set; }

        /// <summary>
        /// 进项收票吨数
        /// </summary>
        [ExcelColumn("进项收票吨数", 9)]
        public double JinXiangShouPiaoDunShu { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [ExcelColumn("库存", 10)]
        public double KuCun { get; set; }

        /// <summary>
        /// 销售类型
        /// </summary>
        [ExcelColumn("销售类型", 11)]
        public string SaleMode { get; set; }

        /// <summary>
        /// 采购单位
        /// </summary>
        [ExcelColumn("采购单位", 12)]
        public string CaiGouDanWei { get; set; }

        /// <summary>
        /// 采购单价
        /// </summary>
        [ExcelColumn("采购单价", 13)]
        public decimal CaiGouDanJia { get; set; }

        /// <summary>
        /// 暂估采购总金额
        /// </summary>
        [ExcelColumn("暂估采购总金额", 14)]
        public decimal ZanGuCaiGouZongJia { get; set; }

        /// <summary>
        /// 采购合同
        /// </summary>
        [ExcelColumn("采购合同", 15)]
        public string CaiGouContractNo { get; set; }

        /// <summary>
        /// 暂估采购不含税金额
        /// </summary>
        [ExcelColumn("暂估采购不含税金额", 16)]
        public decimal ZanGuCaiGouBuHanShuiJinE { get; set; }
    }
}
