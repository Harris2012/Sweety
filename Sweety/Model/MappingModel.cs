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
        public string GoodsNo { get; set; }

        /// <summary>
        /// 合同编号
        /// </summary>
        public string ContractNo { get; set; }
    }
}
