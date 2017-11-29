using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    /// <summary>
    /// 交易模式
    /// </summary>
    public enum BusinessMode
    {
        /// <summary>
        /// 直运
        /// </summary>
        ByHand,

        /// <summary>
        /// 库存销售
        /// </summary>
        FromStore,

        /// <summary>
        /// 入库存
        /// </summary>
        ToStore
    }
}
