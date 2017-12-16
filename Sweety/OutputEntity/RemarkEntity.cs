using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.OutputEntity
{
    [ExcelTable("Remarks")]
    class RemarkEntity
    {
        [ExcelColumn("编号", 1)]
        public string Id { get; set; }

        [ExcelColumn("说明", 2)]
        public string Message { get; set; }
    }
}
