using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sweety.Model
{
    class Remark
    {
        public int RemarkNo { get; set; }

        public string Message { get; set; }

        public Remark(int remarkNo, string message)
        {

        }



        public static Remark FindZeroContractNoInMapping = new Remark(2001, "在商务报表中没有找到对应的销售合同号");
        public static Remark FindMultiContractNoInMapping = new Remark(2002, "在商务报表中找到多余一个销售合同号");
    }
}
