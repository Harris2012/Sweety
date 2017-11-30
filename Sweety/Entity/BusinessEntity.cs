using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.Entity
{
    [ExcelTable("交易表")]
    public class BusinessEntity
    {
        [ExcelColumn("交易号", 1)]
        public string BusinessNo { get; set; }

        [ExcelColumn("名称", 2)]
        public string BusinessName { get; set; }
    }
}
