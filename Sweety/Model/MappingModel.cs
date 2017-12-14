using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    /// <summary>
    /// 商务报表
    /// </summary>
    class MappingModel
    {
        /// <summary>
        /// 数据库编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 合同状态
        /// 1 - 正常
        /// 2 - 撤单
        /// 3 - 还利差
        /// </summary>
        //public int ContractStatus { get; set; }

        /// <summary>
        /// 组别
        /// </summary>
        public string GroupCategory { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public string Applicant { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        public string ProductNo { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNo { get; set; }

        /// <summary>
        /// 购销合同抬头
        /// </summary>
        public string GouXiaoHeTongTaiTou { get; set; }

        /// <summary>
        /// 成交吨数
        /// </summary>
        public double ChengJiaoDunShu { get; set; }

        /// <summary>
        /// 1.买
        /// 2.卖
        /// </summary>
        public int MaiMai { get; set; }

        /// <summary>
        /// 开出/收到发票时间
        /// </summary>
        public DateTime FaPiaoShiJian { get; set; }

        /// <summary>
        /// 发票号码
        /// </summary>
        public string FaPiaoHaoMa { get; set; }
    }
}
