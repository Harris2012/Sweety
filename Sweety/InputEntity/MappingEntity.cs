using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sweety.InputEntity
{
    [ExcelTable("商务报表")]
    class MappingEntity
    {
        [ExcelColumn("数据库编号", 1)]
        public string Id { get; set; }

        [ExcelColumn("合同状态", 5)]
        public string ContractStatus { get; set; }

        [ExcelColumn("组别", 9)]
        public string GroupCategory { get; set; }

        [ExcelColumn("申请人", 10)]
        public string Applicant { get; set; }

        [ExcelColumn("货号", 12)]
        public string ProductNo { get; set; }

        [ExcelColumn("合同编号", 13)]
        public string ContractNo { get; set; }
    }
}
