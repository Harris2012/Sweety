using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Entity
{
    /// <summary>
    /// 原始数据表
    /// </summary>
    public class Origin
    {
        /// <summary>
        /// 交易号
        /// </summary>
        public string BusinessNo { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string PaperNo { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        public string Mode { get; set; }
    }
}
