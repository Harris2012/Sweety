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

        public List<Paper> PaperList { get; set; }
    }
}
