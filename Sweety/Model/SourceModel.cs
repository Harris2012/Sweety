using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    public class SourceModel
    {
        public string PaperNo { get; set; }

        public string BusinessNo { get; set; }

        public string ProductNo { get; set; }

        /// <summary>
        /// 1. 采购
        /// 2. 销售
        /// </summary>
        public int Mode { get; set; }

        public int Count { get; set; }

        public int CreateTime { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
    }
}
