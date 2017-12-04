using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.InputEntity
{
    [ExcelTable("销售合同货号对照表")]
    class MappingEntity
    {
        [ExcelColumn("合同编号", 1)]
        public string ContractNo { get; set; }

        [ExcelColumn("货号", 2)]
        public string GoodsNo { get; set; }
    }
}
