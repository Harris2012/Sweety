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
        public string GouHuoDanWei { get; set; }

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
        public List<string> MappingIds { get; private set; }
        public void SetMappingIds(List<string> mappingIds)
        {
            this.MappingIds = mappingIds;
        }

        /// <summary>
        /// 货号
        /// </summary>
        public string ProductNo { get; private set; }
        public void SetProductNo(string productNo)
        {
            this.ProductNo = productNo;
        }

        /// <summary>
        /// 1. 直运
        /// </summary>
        public int SellMode { get; private set; }
        public void SetSellMode(int sellMode)
        {
            this.SellMode = sellMode;
        }

        /// <summary>
        /// 采购合同号
        /// </summary>
        public string CaiGouContractNo { get; private set; }
        public void SetCaiGouContractNo(string caiGouContractNo)
        {
            this.CaiGouContractNo = caiGouContractNo;
        }

        /// <summary>
        /// 采购单价
        /// </summary>
        public float CaiGouDanJia { get; private set; }
        public void SetCaiGouDanJia(float caiGouDanJia)
        {
            this.CaiGouDanJia = caiGouDanJia;
        }

        /// <summary>
        /// 采购单位
        /// </summary>
        public string CaiGouDanWei { get; set; }
        public void SetCaiGouDanWei(string caiGouDanWei)
        {
            this.CaiGouDanWei = caiGouDanWei;
        }

        /// <summary>
        /// 暂估采购总价
        /// </summary>
        public float ZanGuCaiGouZongJia { get; private set; }
        public void SetZanGuCaiGouZongJia(float zanGuCaiGouZongJia)
        {
            this.ZanGuCaiGouZongJia = zanGuCaiGouZongJia;
        }

        /// <summary>
        /// 暂估采购不含税金额
        /// </summary>
        public float ZanGuCaiGouBuHanShuiJinE { get; private set; }
        public void SetZanGuCaiGouBuHanShuiJinE(float zanGuCaiGouBuHanShuiJinE)
        {
            this.ZanGuCaiGouBuHanShuiJinE = zanGuCaiGouBuHanShuiJinE;
        }

        /// <summary>
        /// 库存
        /// </summary>
        public float KuCun { get; private set; }
        public void SetKuCun(float kuCun)
        {
            this.KuCun = kuCun;
        }

        /// <summary>
        /// 进项收票吨数
        /// </summary>
        public float JinXiangShouPiaoDunShu { get; private set; }
        public void SetJinXiangShouPiaoDunShu(float jinXiangShouPiaoDunShu)
        {
            this.JinXiangShouPiaoDunShu = jinXiangShouPiaoDunShu;
        }
        #endregion
    }
}
