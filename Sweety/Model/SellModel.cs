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

        /// <summary>
        /// 销售合同号
        /// </summary>
        public string SellContractNo { get; set; }

        /// <summary>
        /// 购货单位
        /// </summary>
        public string CaiGouDanWei { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public float ProductCount { get; set; }

        /// <summary>
        /// 商品含税单价
        /// </summary>
        public float ShangPinHanSuiDanJia { get; set; }

        /// <summary>
        /// 商品含税金额
        /// </summary>
        public float ShangPinHanShuiJinE { get; set; }

        /// <summary>
        /// 发票号
        /// </summary>
        public string FaPiaoHao { get; set; }

        #region 日志
        public List<Remark> Remarks { get; private set; }

        public List<string> Messages { get; set; }
        #endregion

        #region 中间过程
        /// <summary>
        /// 商务报表中的数据库编号
        /// </summary>
        public List<string> MappingIds { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 1. 直运
        /// </summary>
        public int SellMode { get; set; }
        #endregion
    }
}
