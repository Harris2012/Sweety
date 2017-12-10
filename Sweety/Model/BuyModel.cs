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
            Messages = new List<string>();
            Remarks = new List<Remark>();
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
        /// Number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 销方名称
        /// </summary>
        public string 销方名称 { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string 组别 { get; set; }

        /// <summary>
        /// 收票号码
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 发票金额
        /// </summary>
        public float BillMoney { get; set; }

        /// <summary>
        /// 不含税金额
        /// </summary>
        public float WithoutTaxMoney { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        public float TaxMoney { get; set; }

        /// <summary>
        /// 收票吨数
        /// </summary>
        public float ReceiveTicketCount { get; set; }

        #region 日志
        public List<Remark> Remarks { get; private set; }

        public List<string> Messages { get; private set; }
        #endregion

        #region 中间过程变量
        /// <summary>
        /// 1. 直运
        /// </summary>
        public int SellStatus { get; set; }
        #endregion
    }
}
