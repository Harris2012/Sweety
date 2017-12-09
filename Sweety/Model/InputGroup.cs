using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    class InputGroup
    {
        /// <summary>
        /// 本期进项明细
        /// </summary>
        public List<BuyModel> BuyModelList { get; set; }

        /// <summary>
        /// 本期销项明细
        /// </summary>
        public List<SellModel> SellModelList { get; set; }

        /// <summary>
        /// 商务报表
        /// </summary>
        public List<MappingModel> MappingModelList { get; set; }
    }
}
