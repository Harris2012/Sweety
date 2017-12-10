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
        public string XiaoShouContractNo { get; set; }

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

        /// <summary>
        /// 购货单位
        /// </summary>
        public string GouHuoDanWei { get; set; }

        /// <summary>
        /// 采购合同号
        /// </summary>
        public string CaiGouContractNo { get; set; }

        /// <summary>
        /// 采购单价
        /// </summary>
        public float CaiGouDanJia { get; set; }

        /// <summary>
        /// 暂估采购总价
        /// </summary>
        public float ZanGuCaiGouZongJia { get; internal set; }

        /// <summary>
        /// 暂估采购不含税金额
        /// </summary>
        public float ZanGuCaiGouBuHanShuiJinE { get; internal set; }

        /// <summary>
        /// 库存
        /// </summary>
        public float KuCun { get; internal set; }

        /// <summary>
        /// 进项收票吨数
        /// </summary>
        public double JinXiangShouPiaoDunShu { get; internal set; }
        #endregion
    }
}
