using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    /// <summary>
    /// 单据
    /// </summary>
    public class Paper
    {
        /// <summary>
        /// 单据号
        /// </summary>
        public string PaperNo { get; set; }

        /// <summary>
        /// 采购数
        /// </summary>
        public int BuyCount { get; set; }

        /// <summary>
        /// 销售数
        /// </summary>
        public int SellCount { get; set; }
    }
}
