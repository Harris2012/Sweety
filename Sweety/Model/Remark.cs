using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    class Remark
    {
        public int RemarkNo { get; private set; }

        public string Message { get; private set; }

        public Remark(int remarkNo, string message)
        {
            this.RemarkNo = remarkNo;
            this.Message = message;
        }

        public static Remark FindZeroContractNoInMapping = new Remark(2001, "用原始的销售合同号在商务报表中没有找到对应的记录");
        public static Remark FindMultiContractNoInMapping = new Remark(2002, "用原始的销售合同号在商务报表中找到多条对应的记录");
        public static Remark FindOneContractNoInMappingUsingSecondContractId = new Remark(2003, "用去掉'-2'之后的销售合同号在商务报表中找到1条对应的记录");
        public static Remark FindMultiContractNoInMappingUsingSecondContractId = new Remark(2004, "用去掉'-2'之后的销售合同号在商务报表中找到N条对应的记录");
        public static Remark UsingPossibleProductNo = new Remark(2005, "使用的是推断出来的货号，可能不准确");

    }
}
