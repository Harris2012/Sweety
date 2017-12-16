using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    /// <summary>
    /// 本期进项明细
    /// </summary>
    class BuyModel
    {
        public BuyModel()
        {
            Remarks = new List<Remark>();
            RelativeBusinessIds = new List<string>();
        }

        /// <summary>
        /// 序号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 采购合同号
        /// </summary>
        public string BuyContractNo { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string BianHao { get; set; }

        /// <summary>
        /// 销方名称
        /// </summary>
        public string XiaoFangMinCheng { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string ZuBie { get; set; }

        /// <summary>
        /// 收票号码
        /// </summary>
        public string ShouPiaoHaoMa { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        public decimal FaPiaoJinE { get; set; }

        /// <summary>
        /// 不含税金额
        /// </summary>
        public decimal BuHanShuiJinE { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        public decimal TaxMoney { get; set; }

        /// <summary>
        /// 收票吨数
        /// </summary>
        public double ReceiveTicketCount { get; set; }

        #region 日志
        public List<Remark> Remarks { get; private set; }
        #endregion

        #region 中间过程变量
        /// <summary>
        /// 1. 直运
        /// </summary>
        public BuyModelSaleMode SellMode { get; private set; }
        public void SetSellMode(BuyModelSaleMode sellMode)
        {
            this.SellMode = sellMode;
        }

        /// <summary>
        /// 商品含税单价
        /// </summary>
        public decimal ShangPinHanShuiDanJia { get; set; }
        public void SetShangPinHanShuiDanJia(decimal shangPinHanShuiDanJia)
        {
            this.ShangPinHanShuiDanJia = shangPinHanShuiDanJia;
        }

        /// <summary>
        /// 商务报表中的相关记录编号
        /// </summary>
        public List<string> RelativeBusinessIds { get; private set; }
        #endregion
    }
}
