using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.InputEntity
{
    [ExcelTable("本期销项明细")]
    class SellEntity
    {
        /// <summary>
        /// 销售合同号
        /// </summary>
        [ExcelColumn("销售合同号", 1)]
        public string SellContractNo { get; set; }

        /// <summary>
        /// 购货单位
        /// </summary>
        [ExcelColumn("购货单位", 2)]
        public string GouHuoDanWei { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [ExcelColumn("商品名称", 3)]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        [ExcelColumn("商品数量", 4)]
        public double ProductCount { get; set; }

        /// <summary>
        /// 商品含税单价
        /// </summary>
        [ExcelColumn("商品含税单价", 5)]
        public decimal ShangPinHanShuiDanJia { get; set; }

        /// <summary>
        /// 商品含税金额
        /// </summary>
        [ExcelColumn("商品含税金额", 6)]
        public decimal ShangPinHanShuiJinE { get; set; }

        /// <summary>
        /// 发票号
        /// </summary>
        [ExcelColumn("发票号", 7)]
        public string FaPiaoHao { get; set; }

    }
}
