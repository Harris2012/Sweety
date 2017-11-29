using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    /// <summary>
    /// 生意
    /// </summary>
    public class Business
    {
        /// <summary>
        /// 交易号
        /// </summary>
        public string BusinessNo { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductNo { get; set; }

        public List<Paper> PaperList { get; set; }
    }
}
