using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    /// <summary>
    /// 本期销项明细销售模式
    /// </summary>
    enum SellModelSaleMode
    {
        /// <summary>
        /// 未知
        /// </summary>
        None = 0,

        /// <summary>
        /// 直运
        /// </summary>
        DirectBusiness = 1,

        /// <summary>
        /// 结算前期库存
        /// </summary>
        JieSuanQianQiKuCun = 2
    }
}
