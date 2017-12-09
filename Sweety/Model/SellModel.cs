using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    /// <summary>
    /// 销项明细
    /// </summary>
    class SellModel
    {
        public SellModel()
        {
            Remarks = new List<Remark>();
        }

        public string SellContractNo { get; set; }
        public string BuProductOrganization { get; set; }
        public string ProductName { get; set; }
        public float ProductCount { get; set; }
        public float ProductUnitPriceWithTax { get; set; }
        public float ProductMoneyWithTax { get; set; }
        public string BillNo { get; set; }

        #region 日志
        /// <summary>
        /// 本条数据是否已经处理完成
        /// </summary>
        public bool IsDone { get; set; }

        public List<Remark> Remarks { get; private set; }

        public List<string> Messages { get; set; }
        #endregion

        #region 中间过程
        public string ProductNo { get; set; }
        #endregion
    }
}
